using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public CombatManager manager;
    public EnemyType enemyType;
    public Enemystates state;
    public int priority = 1;
    public int occupySpace = 1;
    public bool playerInSight;
    public float visionRadius;
    public float visionConeWidth;
    public LayerMask layerMask;

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
}

public enum EnemyType
{
    Normal,
    Ranged,
    Large
}
