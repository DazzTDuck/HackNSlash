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
            if (canAttack)
            {
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
        animator.SetTrigger("whack");
        yield return new WaitForSeconds(attackDuration);
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterHealth>())
        {
            other.GetComponent<CharacterHealth>().TakeDamage(damage);
        }
    }
}
