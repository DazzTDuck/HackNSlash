using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActor : MonoBehaviour
{
    [Header("General")]
    public EnemyType enemyType;
    public Enemystates state;
    public int priority = 1;
    public int occupySpace = 1;
    public Transform model;
    public Animator animator;
    public float animationSmoothing;
    Vector3 animationVector;
    bool isDead;
    [Header("Line of sight")]
    public bool playerInSight;
    public float visionRadius;
    public float visionConeWidth;
    public LayerMask layerMask;
    bool canSee;
    [Header("Backing off")]
    public int hpThreshold;
    public int attackThreshold;
    int currentAttacks;
    bool backedOff;
    public bool backoff = false;

    public float deAggroDistance;
    public float maxDeAggroDistance;
    [Header("Attacking")]
    public float minAttackTime;
    public float maxAttackTime;
    public int attackDamage;
    public float attackDuration;
    public float attackRange;
    float attackTiming;

    NavMeshAgent agent;

    ActorPatrolling patrolling;
    ActorEngaged engaged;
    ActorREngage rangeEngage;
    ActorBackup backup;
    ActorAttacking attacking;
    EnemyRunningSounds sounds;
    bool walkSound;

    private void Start()
    {
        EventsManager.instance.HealthUpdateEvent += CheckToBackoff;
        agent = GetComponentInChildren<NavMeshAgent>();
        patrolling = GetComponentInChildren<ActorPatrolling>();
        patrolling.StartPatrolling();
        engaged = GetComponentInChildren<ActorEngaged>();
        engaged.enabled = false;
        backup = GetComponentInChildren<ActorBackup>();
        if (backup)
            backup.enabled = false;
        attacking = GetComponentInChildren<ActorAttacking>();
        attacking.enabled = false;
        sounds = GetComponent<EnemyRunningSounds>();
        canSee = true;
    }
    private void OnDestroy()
    {
        CombatManager.combatManager.RemoveFromCombat(this);
        EventsManager.instance.HealthUpdateEvent -= CheckToBackoff;
    }
    void CheckToBackoff(object sender = null, HealthUpdateArgs e = null)
    {
        if ((GameObject)sender == gameObject && e?.currentAmount <= 0)
        {
            //TurnInactive();
        }
        else if (state == Enemystates.Partoling && (GameObject)sender == gameObject && e.currentAmount != e.maxAmount)
        {
            CombatManager.combatManager.EnemyJoinsFight(this);
        }
        else if ((state == Enemystates.Engaged || state == Enemystates.Attacking) && !backedOff)
        {
            bool backingOff = false;
            if ((GameObject)sender == gameObject)
            {
                if (e.currentAmount <= hpThreshold)
                    backingOff = CombatManager.combatManager.CheckToFallBack(this);
            }
            else if (currentAttacks >= attackThreshold)
            {
                backingOff = CombatManager.combatManager.CheckToFallBack(this);
                if (backingOff)
                    currentAttacks = 0;
            }
            if (backingOff)
            {
                backedOff = true;
                backoff = true;
            }
        }
    }

    private void FixedUpdate()
    {
        animationVector = Vector3.Lerp(animationVector, agent.velocity, Time.fixedDeltaTime * animationSmoothing);
        animator?.SetFloat("Blend", animationVector.magnitude / agent.speed);

        if (agent.velocity.magnitude > 0.1f && !walkSound)
        {
            sounds.StartRunning();
            walkSound = true;
        }
        else if (agent.velocity.magnitude <= 0.1f && walkSound)
        {
            sounds.StopRunning();
            walkSound = false;
        }

        if (state == Enemystates.Engaged || state == Enemystates.BackUp)
        {
            agent.updateRotation = false;
            transform.LookAt(CombatManager.combatManager.player);
        }
        else
            agent.updateRotation = true;

        if (attackTiming > 0 && Vector3.Distance(transform.position, agent.destination) < 2 && state == Enemystates.Engaged && !GetComponent<ActorStunned>().isStunned)
        {
            float dst = Vector3.Distance(transform.position, CombatManager.combatManager.player.position);
            attackTiming -= Time.fixedDeltaTime;
            if (dst * 1.5f < attackRange)
            {
                float speedMod = ((attackRange - dst * 1.5f) / attackRange) * 3;
                attackTiming -= Time.fixedDeltaTime * speedMod;
            }
        }
        else if (attackTiming <= 0 && state == Enemystates.Engaged && !GetComponent<ActorStunned>().isStunned)
            Attack();

        if (canSee)
            CheckInSight();
        else if (playerInSight)
            playerInSight = false;
        if (state != Enemystates.Partoling)
            CheckAggroDistance();
    }
    void CheckInSight()
    {
        if (Vector3.Distance(transform.position, CombatManager.combatManager.player.position) < visionRadius && Vector3.Dot(transform.forward, CombatManager.combatManager.player.position - transform.position) > (1 - (visionConeWidth / 180f)))
        {
            Vector3 dir = CombatManager.combatManager.player.position - transform.position;
            if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, visionRadius, layerMask))
            {
                if (hit.collider.transform == CombatManager.combatManager.player)
                {
                    playerInSight = true;
                    if (state == Enemystates.Partoling)
                        CombatManager.combatManager.EnemyJoinsFight(this);
                    return;
                }

            }

        }
        playerInSight = false;
    }
    void CheckAggroDistance()
    {
        Transform player = CombatManager.combatManager.player;
        Vector3 origin = patrolling.patrolPosition;
        if (Vector3.Distance(player.position, origin) > maxDeAggroDistance || (Vector3.Distance(player.position, origin) > deAggroDistance && !playerInSight))
            CombatManager.combatManager.RemoveFromCombat(this);
    }

    public void EngagePlayer()
    {
        if (state == Enemystates.Partoling)
        {
            patrolling.StopAllCoroutines();
            patrolling.enabled = false;
        }
        else if (state == Enemystates.BackUp && backup)
        {
            backup.enabled = false;
            backoff = false;
        }
        else if (state == Enemystates.Attacking)
        {
            attacking.enabled = false;
            currentAttacks++;
            CheckToBackoff();
        }
        attackTiming = Random.Range(minAttackTime, maxAttackTime);
        state = Enemystates.Engaged;
        engaged.enabled = true;
        engaged.Engage();
        if (backoff)
            GoBackup();
    }
    public void GoBackup()
    {
        backoff = false;
        if (state == Enemystates.Partoling)
        {
            patrolling.StopAllCoroutines();
            patrolling.enabled = false;
        }
        else if (state == Enemystates.Engaged)
        {
            engaged.Disengage();
            engaged.enabled = false;
        }
        state = Enemystates.BackUp;
        if (backup)
            backup.enabled = true;
    }
    public void RangedEngage()
    {
        if (state == Enemystates.Partoling)
        {
            patrolling.StopAllCoroutines();
            patrolling.enabled = false;
        }
        else if (state == Enemystates.Attacking)
            attacking.enabled = false;
        state = Enemystates.Engaged;
        rangeEngage.enabled = true;
        attackTiming = Random.Range(minAttackTime, maxAttackTime);
    }
    public void ReturnToPatrol()
    {
        if (state == Enemystates.BackUp && backup)
            backup.enabled = false;
        else if (state == Enemystates.Engaged)
        {
            engaged.Disengage();
            engaged.enabled = false;
        }
        else if (state == Enemystates.Attacking)
        {
            attacking.StopAllCoroutines();
            attacking.enabled = false;
        }
        state = Enemystates.Partoling;
        patrolling.enabled = true;
        patrolling.StartPatrolling();
        canSee = false;
        Invoke("CanSee", 1.5f);
    }
    void CanSee()
    {
        canSee = true;
    }    

    public void Attack()
    {
        engaged.StopAllCoroutines();
        engaged.enabled = false;
        state = Enemystates.Attacking;
        attacking.enabled = true;
        attacking.Attack(this, attackDamage, attackRange, attackDuration);
    }

    public void TurnInactive()
    {
        if (state == Enemystates.Attacking)
            attacking.StopAllCoroutines();
        else if (state == Enemystates.Partoling)
            patrolling.StopAllCoroutines();
        attacking.enabled = false;
        engaged.enabled = false;
        if (backup)
            backup.enabled = false;
        agent.isStopped = true;
        state = Enemystates.Dead;
        isDead = true;
        StartCoroutine(Dead());
        CombatManager.combatManager.EnemyDied(this);
    }

    IEnumerator Dead()
    {
        while (isDead)
        {
            state = Enemystates.Dead;
            yield return new WaitForFixedUpdate();
        }
    }
    public void TurnActive()
    {
        transform.position = patrolling.patrolPosition;
        isDead = false;
        GetComponent<Collider>().enabled = true;
        agent.isStopped = false;
        agent.destination = patrolling.patrolPosition;
        GetComponent<CharacterHealth>()?.ReturnToMaxHP();
        playerInSight = false;
        GetComponentInChildren<EnemyDissolve>().ResetMaterials();
        ReturnToPatrol();
    }
}

public enum EnemyType
{
    Normal,
    Ranged,
    Large
}
