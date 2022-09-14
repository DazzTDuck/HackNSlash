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

    public void GetStunned(EnemyBehaviorControll behaviorControll_, float stunDuration)
    {
        agent = GetComponent<NavMeshAgent>();
        behaviorControll = behaviorControll_;
        agent.SetDestination(transform.position);
        StartCoroutine(OhNoImStunned(stunDuration));
        //animator.SetBool("IsStunned", true);
        //animator.SetTrigger("GetStunned");

    }

    IEnumerator OhNoImStunned(float stunduration)
    {
        yield return new WaitForSeconds(stunduration * ((100f - stunResist) / 100f));
        //animator.SetBool("IsStunned", false);
        behaviorControll.ReturnFromStunned();
    }
}