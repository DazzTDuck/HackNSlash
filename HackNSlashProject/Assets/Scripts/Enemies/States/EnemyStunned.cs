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

    public void GetStunned(EnemyBehaviorControll behaviorControll_, float stunDuration, float staggerDistance, float staggerDuration, Transform player)
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        behaviorControll = behaviorControll_;
        agent.isStopped = true;
        Vector3 staggerDir = (transform.position - player.position).normalized;
        StartCoroutine(StaggerKnockBack(stunDuration, staggerDistance, staggerDuration, staggerDir));
    }

    IEnumerator StaggerKnockBack(float stunDuration, float staggerDistance, float staggerDuration, Vector3 staggerDir)
    {

        for (float f = 0; f < staggerDuration; f += Time.fixedDeltaTime)
        {
            rb.AddForce(staggerDir * staggerDistance / (staggerDuration / Time.fixedDeltaTime), ForceMode.Impulse);
            yield return new WaitForFixedUpdate();
        }
        rb.velocity = Vector3.zero;
        StartCoroutine(OhNoImStunned(stunDuration));
        //animator.SetBool("IsStunned", true);
        //animator.SetTrigger("GetStunned");
    }

    IEnumerator OhNoImStunned(float stunduration)
    {
        agent.isStopped = false;
        agent.destination = transform.position;
        yield return new WaitForSeconds(stunduration * ((100f - stunResist) / 100f));
        //animator.SetBool("IsStunned", false);
        behaviorControll.ReturnFromStunned();
    }
}
