using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolyWater : MonoBehaviour
{
    public float delay;
    public float lockoutAfter;
    public float knockBack;
    public float duration;
    public int MaxUses;
    public int remainingUses;
    public int radius;
    public int damage;
    public LayerMask enemylayer;
    public Animator animator;
    PlayerMovement player;

    public float qTime;
    bool attackQ;

    private void Start()
    {
        remainingUses = MaxUses;
    }
    public void UseHolyWater(PlayerMovement player_)
    {
        player = player_;
        if (remainingUses > 0 && !attackQ)
            StartCoroutine(AttackQueue());
    }
    IEnumerator AttackQueue()
    {
        attackQ = true;
        for (float f = 0; f < qTime; f += Time.deltaTime)
        {
            if (player.canAct)
            {
                StartCoroutine(HolyWaterSplash());
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }
    IEnumerator HolyWaterSplash()
    {
        player.canAct = false;
        //animator.SetTrigger();
        yield return new WaitForSeconds(delay);
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, enemylayer);
        foreach (Collider enemyColider in enemies)
        {
            enemyColider.GetComponentInParent<ActorStunned>()?.GetStunned(duration, knockBack, transform);
            enemyColider.GetComponentInParent<CharacterHealth>()?.TakeDamage(damage);
        }
        remainingUses--;
        yield return new WaitForSeconds(lockoutAfter);
        player.canAct = true;
    }
}
