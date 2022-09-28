using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementPoint : MonoBehaviour
{
    public bool occupied;
    public float occupiedRad;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;
    public Transform rayOrigin;

    private void FixedUpdate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, occupiedRad, enemyLayer);
        Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, Vector3.Distance(transform.position, rayOrigin.position), wallLayer);
        if (enemies.Length > 0 || hit.collider)
            occupied = true;
        else 
            occupied = false;

        if (occupied)
            GetComponent<MeshRenderer>().material.color = Color.blue;
        else
            GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
