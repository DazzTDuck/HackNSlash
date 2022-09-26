using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagementPoint : MonoBehaviour
{
    public bool occupied;
    public float occupiedRad;
    public LayerMask enemyLayer;
    public LayerMask wallLayer;

    private void FixedUpdate()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, occupiedRad, enemyLayer);
        if (enemies.Length > 0)
            occupied = true;
        else 
            occupied = false;

        if (occupied)
            GetComponent<MeshRenderer>().material.color = Color.blue;
        else
            GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
