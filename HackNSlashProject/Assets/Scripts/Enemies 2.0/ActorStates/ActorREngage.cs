using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorREngage : MonoBehaviour
{
    NavMeshAgent agent;
    EnemyActor actor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        actor = GetComponent<EnemyActor>();
    }
    void FixedUpdate()
    {
        Vector3 dir = transform.position - CombatManager.combatManager.player.position;
        float dst = actor.attackRange;
        agent.SetDestination(CombatManager.combatManager.player.position + dir.normalized * dst);
    }
}
