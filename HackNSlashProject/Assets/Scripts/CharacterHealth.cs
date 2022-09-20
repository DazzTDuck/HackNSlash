using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



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

        EventsManager.instance.InvokeOnDamageEvent(currentHP, 0, maxHP, gameObject);
    }
    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
