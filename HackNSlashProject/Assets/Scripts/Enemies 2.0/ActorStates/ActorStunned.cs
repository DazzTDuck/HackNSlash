using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorStunned : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rb;
    public bool isStunned;
    public void GetStunned(float stunDuration, float stunPower, Transform player)
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        Vector3 staggerDir = (transform.position - player.position).normalized;
        StartCoroutine(IsStunned(stunDuration, stunPower, staggerDir));
    }

    IEnumerator IsStunned(float stunDuration, float stunPower, Vector3 staggerDir)
    {
        rb.isKinematic = false;
        isStunned = true;
        //animator.SetBool("IsStunned", true);
        //animator.SetTrigger("GetStunned");
        rb.AddForce(staggerDir * stunPower, ForceMode.Impulse);
        yield return new WaitForSeconds(stunDuration);
        rb.isKinematic = true;
        agent.isStopped = false;
        agent.destination = transform.position;
        isStunned = false;
    }
}
