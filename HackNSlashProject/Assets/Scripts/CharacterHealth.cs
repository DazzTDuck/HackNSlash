using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int characterId;

    private void Start()
    {
        currentHP = maxHP;
    }
    public void TakeDamage(int damageToDo)
    {
        if (currentHP - damageToDo <= 0)
        {
            //death
            currentHP = 0;
            Death();
        }
        else
            currentHP -= damageToDo;

        EventsManager.instance.InvokeOnBarEvent(currentHP, 0, maxHP, gameObject);
    }
    protected virtual void Death()
    {
        if (!gameObject.CompareTag("Player"))
            GetComponent<EnemyActor>().TurnInactive();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMaxHP()
    {
        StartCoroutine(HealToFull(maxHP - currentHP));
    }
    IEnumerator HealToFull(int missingHp)
    {
        for (int i = 0; i < missingHp; i++)
        {
            yield return new WaitForFixedUpdate();
            if (currentHP < maxHP)
            {
                currentHP++;
                EventsManager.instance.InvokeOnBarEvent(currentHP, 0, maxHP, gameObject);
            }
        }
    }
}
