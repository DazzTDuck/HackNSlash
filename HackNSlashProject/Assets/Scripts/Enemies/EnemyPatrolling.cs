using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolling : MonoBehaviour
{
    NavMeshAgent agent;
    public PatrolWaypoint[] patrolWaypoints;
    public float dstToWait;
    public float minWaitTime;
    public float maxWaitTime;
    public float wpDeviation;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindWaypoint();
    }

    void FindWaypoint()
    {
        int totalWeight = 0;
        for (int i = 0; i < patrolWaypoints.Length; i++)
            totalWeight += patrolWaypoints[i].actualWeight;
        int weight = Random.Range(0, totalWeight);
        for (int i = 0; i < patrolWaypoints.Length; i++)
        {
            int w = 0;
            for (int wpi = 0; wpi < i; wpi++)
            {
                w += patrolWaypoints[wpi].actualWeight;
            }
            if (weight <= w)
            {
                GoToWaypoint(i);
                break;
            }
        }
    }
    void GoToWaypoint(int wpIndex)
    {
        Vector3 actualDeviation = new Vector3(Random.Range(-wpDeviation, wpDeviation), 0, Random.Range(-wpDeviation, wpDeviation));
        agent.SetDestination(patrolWaypoints[wpIndex].transform.position + actualDeviation);
        StartCoroutine(ArrivedAtWaypoint());
    }

    IEnumerator ArrivedAtWaypoint()
    {
        while (Vector3.Distance(transform.position, agent.destination) > dstToWait)
        {
            yield return new WaitForSeconds(0.5f);
        }
        float wait = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(wait);
        FindWaypoint();
    }
}
