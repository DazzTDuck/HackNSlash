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
    [Header("Line of sight")]
    public bool playerInSight;
    public float visionRadius;
    public float visionConeWidth;
    public LayerMask layerMask;
    [Header("Backing off")]
    public int hpThreshold;
    public int attackThreshold;
    int currentAttacks;
    public bool backedOff;

    private void Start()
    {
        EventsManager.instance.OnBarUpdateEvent += CheckToBackoff;
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
        else if (state == Enemystates.Partoling)
        {
            CombatManager.combatManager.EnemyJoinsFight(this);
        }
        else if (state == Enemystates.Engaged)
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

    }
    public void ReturnToPatrol()
    {

    }
}

public enum EnemyType
{
    Normal,
    Ranged,
    Large
}
