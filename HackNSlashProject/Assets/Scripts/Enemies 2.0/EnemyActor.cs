using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    ActorPatrolling patrolling;
    ActorEngaged engaged;

    private void Start()
    {
        EventsManager.instance.OnBarUpdateEvent += CheckToBackoff;
        patrolling = GetComponent<ActorPatrolling>();
        patrolling.StartPatrolling();
        engaged = GetComponent<ActorEngaged>();
        engaged.enabled = false;
    }
    private void OnDestroy()
    {
        EventsManager.instance.OnBarUpdateEvent -= CheckToBackoff;
        CombatManager.combatManager.RemoveFromCombat(this);
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

        //if (state == Enemystates.Engaged || state == Enemystates.Engaged)
        //    model.LookAt(CombatManager.combatManager.player);
        //else
        //    model.LookAt(transform.position + transform.forward);
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

        }
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

    }
    public void ReturnToPatrol()
    {
        if (state == Enemystates.BackUp)
        {

        }
        else if (state == Enemystates.Engaged)
        {
            engaged.Disengage();
            engaged.enabled = false;
        }
        state = Enemystates.Partoling;
        patrolling.enabled = true;
        patrolling.StartPatrolling();
    }
    public void MeleeAttack()
    {

    }
    public void RangedAttack()
    {

    }
}

public enum EnemyType
{
    Normal,
    Ranged,
    Large
}
