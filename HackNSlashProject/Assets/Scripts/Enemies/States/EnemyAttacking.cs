using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttacking : MonoBehaviour
{
    Transform player;
    EnemyBehaviorControll behaviorControll;
    public int damage;
    public float attacktime;
    NavMeshAgent agent;
    public Animator animator;

    private void Start()
    {
    }

    public void Attack(Transform player_, EnemyBehaviorControll behaviorControll_)
    {
        agent = GetComponent<NavMeshAgent>();
        player = player_;
        behaviorControll = behaviorControll_;
        transform.LookAt(player.position, Vector3.up);
        agent.destination = transform.position;
        StartCoroutine(Attacking());
    }
    IEnumerator Attacking()
    {
        animator.SetTrigger("whack");
        yield return new WaitForSeconds(attacktime);
        behaviorControll.AttackFinished();
    }
}
