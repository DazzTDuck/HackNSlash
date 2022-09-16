using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunned : MonoBehaviour
{
    public float stunResist;
    EnemyBehaviorControll behaviorControll;
    NavMeshAgent agent;
    public Animator animator;
    Rigidbody rb;

    public void GetStunned(EnemyBehaviorControll behaviorControll_, float stunDuration, float stunPower, Transform player)
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        behaviorControll = behaviorControll_;
        agent.isStopped = true;
        Vector3 staggerDir = (transform.position - player.position).normalized;
        StartCoroutine(IsStunned(stunDuration, stunPower, staggerDir));
    }

    IEnumerator IsStunned(float stunDuration, float stunPower, Vector3 staggerDir)
    {

        //animator.SetBool("IsStunned", true);
        //animator.SetTrigger("GetStunned");
        rb.AddForce(staggerDir * stunPower, ForceMode.Impulse);
        yield return new WaitForSeconds(stunDuration);
        agent.isStopped = false;
        agent.destination = transform.position;
        behaviorControll.ReturnFromStunned();
    }
}
