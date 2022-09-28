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
    public LayerMask enemylayer;
    public Animator animator;
    PlayerMovement player;
    public int powerConsumption;

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
            if (player.canAct && player.holyPower.currentHolyPower >= powerConsumption)
            {
                player.holyPower.UseHolyPower(powerConsumption);
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
            enemyColider.GetComponent<ActorStunned>()?.GetStunned(duration, knockBack, transform);
        }
        remainingUses--;
        yield return new WaitForSeconds(lockoutAfter);
        player.canAct = true;
    }
}
