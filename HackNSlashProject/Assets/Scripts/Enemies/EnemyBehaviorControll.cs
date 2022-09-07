using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviorControll : MonoBehaviour
{
    public float sightRange;
    public float sightAngle;
    public AiState currentState;
    public bool playerInSight;
    public float attackRange;
    public float patrollSpeed;
    public float chaseSpeed;
    NavMeshAgent agent;
    Transform player;
    int id;
    EnemyPatrolling patrolling;
    EnemyChasing chasing;
    EnemyAttacking attacking;
    //EnemyStunned stunned;
    //EnemyStaggered staggered;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        id = GetComponent<CharacterHealth>().characterId;
        patrolling = GetComponent<EnemyPatrolling>();
        chasing = GetComponent<EnemyChasing>();
        attacking = GetComponent<EnemyAttacking>();
        agent = GetComponent<NavMeshAgent>();
        currentState = AiState.Patrolling;
        patrolling.enabled = true;
        chasing.enabled = false;
        attacking.enabled = false;
        agent.speed = patrollSpeed;
        StartCoroutine(StartBehavior());
    }
    IEnumerator StartBehavior()
    {
        yield return null;
        patrolling.FindWaypoint();
    }

    public void ChaseFailed()
    {
        chasing.enabled = false;
        currentState = AiState.Patrolling;
        agent.speed = patrollSpeed;
        patrolling.enabled = true;
        patrolling.FindWaypoint();
    }
    public void ChaseSuccess()
    {
        chasing.enabled = false;
        currentState = AiState.Attacking;
        attacking.enabled = true;
        attacking.Attack(player, this);
    }

    public void AttackFinished()
    {
        attacking.enabled = false;
        currentState = AiState.Chasing;
        chasing.enabled = true;
        chasing.StartChasing(player, this);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, player.position) < sightRange && Vector3.Dot(transform.forward, player.position - transform.position) > (1 - (sightAngle / 180f)))
        {
            Vector3 dir = player.position - transform.position;
            if (Physics.Raycast(transform.position, dir.normalized, out RaycastHit hit, sightRange))
            {
                if (hit.collider.transform == player)
                {
                    playerInSight = true;
                    if (currentState == AiState.Patrolling)
                    {
                        patrolling.StopAllCoroutines();
                        patrolling.enabled = false;
                        currentState = AiState.Chasing;
                        agent.speed = chaseSpeed;
                        chasing.enabled = true;
                        chasing.StartChasing(player, this);
                    }
                }
                else
                {
                    playerInSight = false;
                }
            }
            else
            {
                playerInSight = false;
            }
        }
        else
        {
            playerInSight = false;
        }
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