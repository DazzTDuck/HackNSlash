using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorBackup : MonoBehaviour
{
    EngagementPoint engagementPoint;
    NavMeshAgent agent;
    public float backupDistance;
    public LayerMask enemyLayer;
    public float personalSpaceRadius;
    float timeToMove;
    public float maxMove;
    public Vector2 moveTimeMinMax;

    void Engage()
    {
        agent = GetComponent<NavMeshAgent>();
        EngagementPoint[] points = FindObjectsOfType<EngagementPoint>();
        foreach (EngagementPoint point in points)
        {
            if (!point.occupied && (!engagementPoint || Vector3.Distance(transform.position, point.transform.position) < Vector3.Distance(transform.position, engagementPoint.transform.position)))
                engagementPoint = point;
        }
        timeToMove = Random.Range(4, 6);
    }
    void Flank()
    {
        EngagementPoint[] points = FindObjectsOfType<EngagementPoint>();
        List<EngagementPoint> validPoints = new List<EngagementPoint>();
        foreach (EngagementPoint point in points)
        {
            if (!point.occupied && Vector3.Distance(transform.position, point.transform.position) < maxMove)
                validPoints.Add(point);
        }
        if (validPoints.Count > 0)
        {
            int i = Random.Range(0, validPoints.Count);
            engagementPoint = validPoints[i];
            timeToMove = Random.Range(moveTimeMinMax.x, moveTimeMinMax.y);
        }
        else
            timeToMove = 1;
    }
    void FixedUpdate()
    {
        if (engagementPoint)
            agent.SetDestination(engagementPoint.transform.position);
        else
            Engage();

        if (timeToMove > 0)
            timeToMove -= Time.fixedDeltaTime;
        else if (timeToMove <= 0)
            Flank();

        //Vector3 dir = transform.position - CombatManager.combatManager.player.position;
        //Vector3 eDir = new Vector3();
        //Collider[] enemies = Physics.OverlapSphere(transform.position, personalSpaceRadius, enemyLayer);
        //foreach (Collider collider in enemies)
        //{
        //    eDir += (transform.position - collider.transform.position);
        //}
        //agent.SetDestination(CombatManager.combatManager.player.position + dir.normalized * backupDistance + eDir.normalized);
    }
}
