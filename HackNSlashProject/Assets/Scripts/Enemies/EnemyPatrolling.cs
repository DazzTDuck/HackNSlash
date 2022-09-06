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

    public void FindWaypoint()
    {
        int totalWeight = 0;
        for (int i = 0; i < patrolWaypoints.Length; i++)
            totalWeight += patrolWaypoints[i].actualWeight;
        Debug.Log(totalWeight + "total wight");
        int weight = Random.Range(0, totalWeight);
        Debug.Log(weight + "current wight");
        int w = 0;
        for (int i = 0; i < patrolWaypoints.Length; i++)
        {
            w += patrolWaypoints[i].actualWeight;
            if (weight <= w)
            {
                Debug.Log(i + "waypoint");
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
        float f = 0;
        while (Vector3.Distance(transform.position, agent.destination) > dstToWait && f < 25)
        {
            yield return new WaitForSeconds(0.25f);
            f += 0.25f;
        }
        Debug.Log("Arrived at Waypoint");
        float wait = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(wait);
        FindWaypoint();
    }
}
