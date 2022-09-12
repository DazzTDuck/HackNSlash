using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWaypoint : MonoBehaviour
{
    public int baseWeight = 1;
    public int actualWeight;
    public int playerWeight = 5;
    public float playerRadius = 5;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        StartCoroutine(TestForPlayer());
    }

    IEnumerator TestForPlayer()
    {
        while (true)
        {
            float dst = Vector3.Distance(transform.position, player.position);
            if (dst <= playerRadius)
                actualWeight = baseWeight + playerWeight;
            else
                actualWeight = baseWeight;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
