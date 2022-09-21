using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    [Header("General")]
    public CombatManager manager;
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
    void CheckToBackoff(object sender = null, OnBarArgs e = null)
    {
        if (e?.currentAmount <= 0)
        {
            manager.RemoveFromCombat(this);
        }
        else if (state == Enemystates.Partoling)
        {
            manager.EnemyJoinsFight(this);
        }
        else if (state == Enemystates.Engaged)
        {
            bool backingOff = false;
            if (e != null && (GameObject)sender == gameObject && !backedOff)
            {
                if (e.currentAmount <= hpThreshold)
                {
                    backingOff = manager.CheckToFallBack(this);
                }
            }
            else if (currentAttacks >= attackThreshold)
            {
                backingOff = manager.CheckToFallBack(this);
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
        if (Vector3.Distance(transform.position, manager.player.position) < visionRadius && Vector3.Dot(transform.forward, manager.player.position - transform.position) > (1 - (visionConeWidth / 180f)))
        {
            Vector3 dir = manager.player.position - transform.position;
            if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, visionRadius, layerMask))
            {
                if (hit.collider.transform == manager.player)
                {
                    playerInSight = true;
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
