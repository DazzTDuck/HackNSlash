using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorBackup : MonoBehaviour
{
    NavMeshAgent agent;
    public float backupDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {
        Vector3 dir = transform.position - CombatManager.combatManager.player.position;
        agent.SetDestination(CombatManager.combatManager.player.position + dir.normalized * backupDistance);
    }
}
