using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasing : MonoBehaviour
{
    Transform player;
    Vector3 chasePos;
    EnemyBehaviorControll behaviorControll;
    NavMeshAgent agent;
    //Vector3 chaseDev;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartChasing(Transform player_, EnemyBehaviorControll behaviorControll_)
    {
        player = player_;
        behaviorControll = behaviorControll_;
        agent.destination = player.position;
    }

    private void FixedUpdate()
    {
        if (behaviorControll.playerInSight)
            chasePos = player.position;
        else if (Vector3.Distance(transform.position, agent.destination) < 1)
            behaviorControll.ChaseFailed();
        agent.destination = chasePos;
        if (Vector3.Distance(transform.position, player.position) <= behaviorControll.attackRange)
        {
            behaviorControll.ChaseSuccess();
        }
    }
}
