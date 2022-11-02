using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordAttack : MonoBehaviour
{
    public float attackDuration;
    public float qTime;
    public int damage;
    public Animator animator;
    bool attackQ;
    public int powerConsumption;
    PlayerMovement player;
    public HolySwordVisual swordVisual;
    

    public void Attack(PlayerMovement player_)
    {
        player = player_;
        if (!attackQ)
            StartCoroutine(AttackQueue());
    }
    IEnumerator AttackQueue()
    {
        attackQ = true;
        for (float f = 0; f < qTime; f+= Time.deltaTime)
        {
            if (player.canAct && player.holyPower.currentHolyPower >= powerConsumption)
            {
                player.holyPower.UseHolyPower(powerConsumption);
                StartCoroutine(Swinging());
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }

    IEnumerator Swinging()
    {
        player.canAct = false;
        swordVisual.StartAttackVisual();
        GetComponentInParent<Rigidbody>().isKinematic = true;
        animator.SetTrigger("whack");
        yield return new WaitForSeconds(attackDuration);
        GetComponentInParent<Rigidbody>().isKinematic = false;
        player.canAct = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        int i = Random.Range(-5, 5);
        other.GetComponentInParent<CharacterHealth>()?.TakeDamage(damage + i);
    }
}
