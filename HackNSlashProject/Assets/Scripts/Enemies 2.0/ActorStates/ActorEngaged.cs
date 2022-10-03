using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorEngaged : MonoBehaviour
{
    EngagementPoint engagementPoint;
    EnemyActor actor;
    NavMeshAgent agent;
    float timeToMove;
    public Vector2 engageRangeMinMax;
    public void Engage()
    {
        agent = GetComponent<NavMeshAgent>();
        actor = GetComponent<EnemyActor>();
        //EngagementPoint[] points = FindObjectsOfType<EngagementPoint>();
        //foreach (EngagementPoint point in points)
        //{
        //    if (!point.occupied && (!engagementPoint || Vector3.Distance(transform.position, point.transform.position) < Vector3.Distance(transform.position, engagementPoint.transform.position)))
        //        engagementPoint = point;
        //}
        //timeToMove = Random.Range(4, 6);
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
            timeToMove = Random.Range(4, 6);
        }
        else
            timeToMove = 1;
    }

    private void FixedUpdate()
    {
        //if (engagementPoint)
        //    agent.SetDestination(engagementPoint.transform.position);
        //else
        //    Engage();

        //if (timeToMove > 0)
        //    timeToMove -= Time.fixedDeltaTime;
        //else if (timeToMove <= 0)
        //    Flank();

        Vector3 targetPos = CombatManager.combatManager.player.position;
        Vector3 dir = (transform.position - targetPos).normalized;
        float dst = Vector3.Distance(transform.position, targetPos);
        if (dst > engageRangeMinMax.y || !actor.playerInSight)
            agent.SetDestination(targetPos);
        else if (dst < engageRangeMinMax.x)
            agent.SetDestination(targetPos + dir * (dst + 1.5f));
        else
            agent.SetDestination(transform.position);
    }

    public void Disengage()
    {
        engagementPoint = null;
    }
}
