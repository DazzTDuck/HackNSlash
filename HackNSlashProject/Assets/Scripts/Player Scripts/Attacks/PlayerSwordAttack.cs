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
    Collider hurtBox;

    private void Start()
    {
        hurtBox = GetComponent<Collider>();
        hurtBox.enabled = false;
    }

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
                swordVisual.StartAttackVisual();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }

    IEnumerator Swinging()
    {
        player.canAct = false;
        hurtBox.enabled = true;
        GetComponentInParent<Rigidbody>().isKinematic = true;
        animator.SetTrigger("Attack");
        AudioManager.instance.PlaySound("SwordSlash");
        yield return new WaitForSeconds(attackDuration);
        hurtBox.enabled = false;
        GetComponentInParent<Rigidbody>().isKinematic = false;
        player.canAct = true;
        swordVisual.StopAttackVisual();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            int i = Random.Range(-5, 5);
            other.GetComponentInParent<CharacterHealth>()?.TakeDamage(damage + i);
        }
    }
}
