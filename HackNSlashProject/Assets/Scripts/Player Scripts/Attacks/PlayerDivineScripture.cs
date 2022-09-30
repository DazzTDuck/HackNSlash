using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDivineScripture : MonoBehaviour
{
    public float knockBack;
    public float duration;
    public float delay;
    public float lockoutAfter;
    public LayerMask enemyLayer;
    public float radius;
    public float angle;
    public int damage;

    public float qTime;
    public Animator animator;
    bool attackQ;
    public int powerConsumption;
    PlayerMovement player;


    public void ReadScripture(PlayerMovement player_)
    {
        player = player_;
        if (!attackQ)
            StartCoroutine(AttackQueue());
    }
    IEnumerator AttackQueue()
    {
        attackQ = true;
        for (float f = 0; f < qTime; f += Time.deltaTime)
        {
            if (player.canAct && player.holyPower.currentHolyPower >= powerConsumption)
            {
                player.holyPower.UseHolyPower(powerConsumption);
                StartCoroutine(Begone());
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }

    IEnumerator Begone()
    {
        player.canAct = false;
        //animator.SetTrigger();
        yield return new WaitForSeconds(delay);
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (Collider enemyColider in enemies)
        {
            if (Vector3.Dot(transform.forward, enemyColider.transform.position - transform.position) > (1 - (angle / 180f)))
            {
                enemyColider.GetComponentInParent<ActorStunned>()?.GetStunned(duration, knockBack, transform);
                enemyColider.GetComponentInParent<CharacterHealth>()?.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(lockoutAfter);
        player.canAct = true;
    }
}
