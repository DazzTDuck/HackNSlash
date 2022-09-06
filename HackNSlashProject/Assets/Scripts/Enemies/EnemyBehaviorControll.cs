using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorControll : MonoBehaviour
{
    public float sightRange;
    public float sightAngle;
    public AiState currentState;
    Transform player;
    int id;
    EnemyPatrolling patrolling;
    //EnemyChasing chasing;
    //EnemyAttacking attacking;
    //EnemyStunned stunned;
    //EnemyStaggered staggered;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        id = GetComponent<CharacterHealth>().characterId;
        patrolling = GetComponent<EnemyPatrolling>();
    }
}
public enum AiState
{
    Patrolling,
    Chasing,
    Attacking,
    Stunned,
    Staggered,
    Dead
}