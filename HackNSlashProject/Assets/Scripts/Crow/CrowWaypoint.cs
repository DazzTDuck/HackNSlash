using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowWaypoint : MonoBehaviour
{
    public CrowWaypoint[] adjacentWaypoints;
    public CrowAI attachedCrow;
    public float scareDistance;
    public bool isScary;
    Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < scareDistance)
        {
            isScary = true;
            if (attachedCrow)
            {
                attachedCrow.LookForNewPosition();
            }
        }
        else
            isScary = false;
    }
}
