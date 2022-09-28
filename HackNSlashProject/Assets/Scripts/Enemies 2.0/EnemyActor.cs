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
    [Header("Line of sight")]
    public bool playerInSight;
    public float visionRadius;
    public float visionConeWidth;
    public LayerMask layerMask;
    [Header("Backing off")]
    public int hpThreshold;
    public int attackThreshold;
    int currentAttacks;
    bool backedOff;
    public bool backoff = false;
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
    ActorAttacking attacking;

    private void Start()
    {
        EventsManager.instance.OnBarUpdateEvent += CheckToBackoff;
        agent = GetComponent<NavMeshAgent>();
        patrolling = GetComponent<ActorPatrolling>();
        patrolling.StartPatrolling();
        if (enemyType == EnemyType.Ranged)
        {
            rangeEngage = GetComponent<ActorREngage>();
            rangeEngage.enabled = false;
        }
        else
        {
            engaged = GetComponent<ActorEngaged>();
            engaged.enabled = false;
        }
        attacking = GetComponent<ActorAttacking>();
        attacking.enabled = false;
    }
    private void OnDestroy()
    {
        CombatManager.combatManager.RemoveFromCombat(this);
        EventsManager.instance.OnBarUpdateEvent -= CheckToBackoff;
    }
    void CheckToBackoff(object sender = null, OnBarArgs e = null)
    {
        if ((GameObject)sender == gameObject && e?.currentAmount <= 0)
        {
            CombatManager.combatManager.RemoveFromCombat(this);
        }
        else if (state == Enemystates.Partoling && (GameObject)sender == gameObject)
        {
            CombatManager.combatManager.EnemyJoinsFight(this);
        }
        else if (state == Enemystates.Engaged || state == Enemystates.Attacking)
        {
            bool backingOff = false;
            if ((GameObject)sender == gameObject && !backedOff)
            {
                if (e.currentAmount <= hpThreshold)
                {
                    backingOff = CombatManager.combatManager.CheckToFallBack(this);
                }
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
        if (state == Enemystates.Engaged || state == Enemystates.BackUp)
        {
            agent.updateRotation = false;
            transform.LookAt(CombatManager.combatManager.player);
        }
        else
            agent.updateRotation = true;

        if (attackTiming > 0 && Vector3.Distance(transform.position, agent.destination) < 2 && state == Enemystates.Engaged)
            attackTiming -= Time.fixedDeltaTime;
        else if (attackTiming <= 0 && state == Enemystates.Engaged)
        {
            Attack();
        }

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
    public void EngagePlayer()
    {
        if (state == Enemystates.Partoling)
        {
            patrolling.StopAllCoroutines();
            patrolling.enabled = false;
        }
        else if (state == Enemystates.BackUp)
        {

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
        {
            GoBackup();
        }
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
        if (state == Enemystates.BackUp)
        {

        }
        else if (state == Enemystates.Engaged && enemyType != EnemyType.Ranged)
        {
            engaged.Disengage();
            engaged.enabled = false;
        }
        else if (state == Enemystates.Engaged && enemyType == EnemyType.Ranged)
        {
            rangeEngage.enabled = false;
        }
        state = Enemystates.Partoling;
        patrolling.enabled = true;
        patrolling.StartPatrolling();
    }
    public void Attack()
    {
        if (enemyType == EnemyType.Ranged)
        {
            rangeEngage.enabled = false;
        }
        else
        {
            engaged.StopAllCoroutines();
            engaged.enabled = false;
        }
        state = Enemystates.Attacking;
        attacking.enabled = true;
        attacking.Attack(this, attackDamage, attackRange, attackDuration);
    }
}

public enum EnemyType
{
    Normal,
    Ranged,
    Large
}
