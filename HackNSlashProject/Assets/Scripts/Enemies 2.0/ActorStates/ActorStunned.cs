using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorStunned : MonoBehaviour
{
    NavMeshAgent agent;
    Rigidbody rb;
    public void GetStunned(float stunDuration, float stunPower, Transform player)
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 2;
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        Vector3 staggerDir = (transform.position - player.position).normalized;
        StartCoroutine(IsStunned(stunDuration, stunPower, staggerDir));
    }

    IEnumerator IsStunned(float stunDuration, float stunPower, Vector3 staggerDir)
    {
        rb.isKinematic = false;
        //animator.SetBool("IsStunned", true);
        //animator.SetTrigger("GetStunned");
        rb.AddForce(staggerDir * stunPower, ForceMode.Impulse);
        Debug.Log(1);
        yield return new WaitForSeconds(stunDuration);
        rb.isKinematic = true;
        agent.isStopped = false;
        agent.destination = transform.position;
    }
}
