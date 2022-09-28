using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngagedEnemyPositioning : MonoBehaviour
{
    public float followSpeed;

    private void FixedUpdate()
    {
        Vector3 move = Vector3.Slerp(transform.position, CombatManager.combatManager.player.position, followSpeed * Time.deltaTime);
        transform.position = move;
    }
}
