using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CrowAI : MonoBehaviour
{
    public CrowWaypoint attachedWaypoint;
    Animator animator;
    Vector3 startPos;
    bool idle;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        CrowWaypoint[] waypoints = FindObjectsOfType<CrowWaypoint>();
        foreach (CrowWaypoint wp in waypoints)
        {
            if ((!attachedWaypoint || Vector3.Distance(wp.transform.position, transform.position) < Vector3.Distance(attachedWaypoint.transform.position, transform.position)) && !wp.attachedCrow)
                attachedWaypoint = wp;
        }
        FlyToWP();
    }

    public void LookForNewPosition()
    {
        Debug.Log("look for");
        idle = false;
        StopAllCoroutines();
        List<CrowWaypoint> possibleWPs = new List<CrowWaypoint>();
        foreach (CrowWaypoint wp in attachedWaypoint.adjacentWaypoints)
        {
            if (wp.attachedCrow == null && wp.isScary == false)
            {
                possibleWPs.Add(wp);
                Debug.Log(wp.name);
            }
        }
        CrowWaypoint newWaypoint = possibleWPs[Random.Range(0, possibleWPs.Count)];
        attachedWaypoint.attachedCrow = null;
        attachedWaypoint = newWaypoint;
        Debug.Log(newWaypoint.name);
        FlyToWP();
        
    }

    void FlyToWP()
    {
        attachedWaypoint.attachedCrow = this;
        animator.SetTrigger("StartFlying");
        startPos = transform.position;
        Vector3 targetPos = attachedWaypoint.transform.position;
        Vector3 dir = targetPos - startPos;
        Vector3 groundDir = new Vector3(dir.x, 0, dir.z);
        transform.LookAt(transform.position + groundDir);
        targetPos = new Vector3(groundDir.magnitude, dir.y, 0);
        float InitialVelocity;
        float time;
        float angle;
        float height = targetPos.y + targetPos.magnitude / 5f;
        height = Mathf.Max(0.001f, height);
        CalculatePathWithHeight(targetPos, height, out InitialVelocity, out angle, out time);
        StartCoroutine(InAirMoving(groundDir.normalized, InitialVelocity, angle, time));
    }

    float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }
    void CalculatePathWithHeight(Vector3 targetPos, float h, out float initialVelocity, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float a = (-0.5f * g);
        float b = Mathf.Sqrt(2 * g * h);
        float c = -yt;

        float tPlus = QuadraticEquation(a, b, c, 1);
        float tMin = QuadraticEquation(a, b, c, -1);

        time = tPlus > tMin ? tPlus : tMin;
        angle = Mathf.Atan(b * time / xt);
        initialVelocity = b / Mathf.Sin(angle);
    }
    IEnumerator InAirMoving(Vector3 dir, float initialVelocity, float angle, float time)
    {
        float t = 0;
        yield return new WaitForSeconds(0.2f);
        while (t < time)
        {
            float x = initialVelocity * t * Mathf.Cos(angle);
            float y = initialVelocity * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = Vector3.Lerp(transform.position, startPos + dir * x + Vector3.up * y, Time.fixedDeltaTime * 4);

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        animator.SetTrigger("Landing");
        while (Vector3.Distance(transform.position, attachedWaypoint.transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, attachedWaypoint.transform.position, Time.fixedDeltaTime * 4);
            yield return new WaitForFixedUpdate();
        }
        idle = true;
        StartCoroutine(Idling());
    }

    IEnumerator Idling()
    {
        animator.SetInteger("IdleAnim", 0);
        yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
        int scenario = Random.Range(-1, 3);
        animator.SetInteger("IdleAnim", scenario);
        yield return new WaitForSeconds(Random.Range(1f, 2.5f));
        while (idle)
        {
            scenario = Random.Range(-1, 5);
            if (scenario <= 2)
                animator.SetInteger("IdleAnim", scenario);
            else
                LookForNewPosition();
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
    }
}