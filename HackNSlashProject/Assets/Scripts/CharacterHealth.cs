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

        EventsManager.instance.InvokeHealthUpdateEvent(characterId, currentHP, 0, maxHP, gameObject);
    }
    protected virtual void Death()
    {
        if (gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (GetComponent<EnemyActor>())
            GetComponent<EnemyActor>().TurnInactive();
        else
            gameObject.SetActive(false);
    }

    public void ReturnToMaxHP()
    {
        currentHP = maxHP;
        EventsManager.instance.InvokeHealthUpdateEvent(characterId, currentHP, 0, maxHP, gameObject);
    }
}
