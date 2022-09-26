using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorEngaged : MonoBehaviour
{
    EngagementPoint engagementPoint;
    NavMeshAgent agent;
    float timeToMove;
    public void Engage()
    {
        agent = GetComponent<NavMeshAgent>();
        EngagementPoint[] points = FindObjectsOfType<EngagementPoint>();
        foreach (EngagementPoint point in points)
        {
            if (!point.occupied && (!engagementPoint || Vector3.Distance(transform.position, point.transform.position) < Vector3.Distance(transform.position, engagementPoint.transform.position)))
                engagementPoint = point;
        }
        timeToMove = 5;
    }

    void Flank()
    {
        EngagementPoint[] points = FindObjectsOfType<EngagementPoint>();
        List<EngagementPoint> validPoints = new List<EngagementPoint>();
        foreach (EngagementPoint point in points)
        {
            if (!point.occupied && Vector3.Distance(transform.position, point.transform.position) < 5)
                validPoints.Add(point);
        }
        if (validPoints.Count > 0)
        {
            int i = Random.Range(0, validPoints.Count);
            engagementPoint = validPoints[i];
            timeToMove = Random.Range(5, 7);
        }
        else
            timeToMove = Random.Range(3, 5);
    }

    private void FixedUpdate()
    {
        if (engagementPoint)
            agent.SetDestination(engagementPoint.transform.position);
        else
            Engage();

        if (timeToMove > 0)
            timeToMove -= Time.fixedDeltaTime;
        else if (timeToMove <= 0)
            Flank();
    }

    public void Disengage()
    {
        engagementPoint = null;
    }
}
