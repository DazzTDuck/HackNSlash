using System.Collections;
using System.Collections.Generic;
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
        //EventsManager.instance.OnTakeDamage(characterId, damageToDo);

        if (currentHP - damageToDo <= 0)
        {
            //death
            currentHP = 0;
            Death();
        }
        else
            currentHP -= damageToDo;
    }
    public virtual void Death()
    {
        //EventsManager.instance.OnDeath(characterId);
        Destroy(gameObject);
    }
}
