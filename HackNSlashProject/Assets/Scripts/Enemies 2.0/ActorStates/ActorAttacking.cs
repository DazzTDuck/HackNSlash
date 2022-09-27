using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorAttacking : MonoBehaviour
{
    NavMeshAgent agent;
    EnemyActor actor;
    int damage;
    float attackRange;
    float attackDuration;

    public void Attack(EnemyActor actor_, int damage_, float range_, float duration_)
    {
        agent = GetComponent<NavMeshAgent>();
        actor = actor_;
        damage = damage_;
        attackRange = range_;
        attackDuration = duration_;
        agent.SetDestination(CombatManager.combatManager.player.position + (transform.position - CombatManager.combatManager.player.position).normalized * attackRange * 0.5f);
        StartCoroutine(WalkToAttack());
    }
    IEnumerator WalkToAttack()
    {
        while (Vector3.Distance(transform.position, agent.destination) >= attackRange)
        {
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(Attacking());
    }
    protected virtual IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackDuration);
        if (Vector3.Distance(transform.position, CombatManager.combatManager.player.position) <= attackRange)
            CombatManager.combatManager.player.GetComponent<CharacterHealth>().TakeDamage(damage);
        AttackFinished();
    }
    void AttackFinished()
    {
        if (actor.enemyType == EnemyType.Ranged)
            actor.RangedEngage();
        else
            actor.EngagePlayer();

    }
}
