using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStaggered : MonoBehaviour
{
    EnemyBehaviorControll behaviorControll;
    NavMeshAgent agent;
    Rigidbody rb;
    bool getStaggered;

    public void GetStaggered(EnemyBehaviorControll behaviorControll_, float knockBack, Transform player)
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        behaviorControll = behaviorControll_;
        agent.isStopped = true;
        rb.AddForce((player.position - transform.position + Vector3.up *5).normalized * knockBack, ForceMode.Impulse);
        getStaggered = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (getStaggered && collision.collider.tag == "Floor")
        {
            agent.isStopped = false;
            agent.destination = transform.position;
            getStaggered = false;
            behaviorControll.ReturnFromStaggered();
        }
    }
}
