using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorPatrolling : MonoBehaviour
{
    public Vector3 patrolPosition;
    public float patrolDistance;
    public Vector2 waitTimeMinMax;
    NavMeshAgent agent;

    public void StartPatrolling()
    {
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
            patrolPosition = transform.position;
        }
        FindPosition();
    }
    void FindPosition()
    {
        Vector3 moveToPos = patrolPosition + new Vector3(Random.Range(-patrolDistance, patrolDistance), 0, Random.Range(-patrolDistance, patrolDistance));
        agent.SetDestination(moveToPos);
        StartCoroutine(WaitToMove());
    }
    IEnumerator WaitToMove()
    {
        float waitTime = 30;
        while (Vector3.Distance(transform.position, agent.destination) > 1.5f && waitTime > 0)
        {
            yield return new WaitForFixedUpdate();
            waitTime -= Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(Random.Range(waitTimeMinMax.x, waitTimeMinMax.y));
        FindPosition();
    }
}
