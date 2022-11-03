using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorThrowProjectile : ActorAttacking
{
    public float splashRadius;
    public GameObject projectile;
    public Transform throwPosition;

    protected override IEnumerator Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(CombatManager.combatManager.player.position);
        GetComponentInChildren<Animator>().SetTrigger("Throw");
        yield return new WaitForSeconds(attackDuration);
        GameObject liveProjectile = Instantiate(projectile, throwPosition.position, throwPosition.rotation);
        liveProjectile.GetComponent<Projectile>()?.Throw(damage, splashRadius);
        AttackFinished();
    }
}
