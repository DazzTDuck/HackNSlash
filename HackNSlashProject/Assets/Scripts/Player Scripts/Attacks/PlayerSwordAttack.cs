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
    bool canAttack = true;
    bool attackQ;
    public HolyPower holyPower;
    public int powerConsumption;
    

    public void OnAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started && !attackQ)
            StartCoroutine(AttackQueue());
    }
    IEnumerator AttackQueue()
    {
        attackQ = true;
        for (float f = 0; f < qTime; f+= Time.deltaTime)
        {
            if (canAttack && holyPower.currentHolyPower >= powerConsumption)
            {
                holyPower.UseHolyPower(powerConsumption);
                StartCoroutine(Swinging());
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }

    IEnumerator Swinging()
    {
        canAttack = false;
        GetComponentInParent<Rigidbody>().isKinematic = true;
        animator.SetTrigger("whack");
        yield return new WaitForSeconds(attackDuration);
        GetComponentInParent<Rigidbody>().isKinematic = false;
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<CharacterHealth>()?.TakeDamage(damage);
    }
}
