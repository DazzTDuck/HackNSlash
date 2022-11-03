
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterHealth : MonoBehaviour
{
    public int maxHP;
    public int currentHP;
    public int characterId;
    public ParticleManager hurtParticle;
    public ParticleManager deathParticle;

    private void Start()
    {
        currentHP = maxHP;
    }
    public void TakeDamage(int damageToDo)
    {
        if (currentHP - damageToDo <= 0)
        {
            currentHP = 0;
            Death();
        }
        else
            currentHP -= damageToDo;
        hurtParticle.GetParticle(transform);

        EventsManager.instance.InvokeHealthUpdateEvent(characterId, currentHP, 0, maxHP, gameObject);
    }
    protected virtual void Death()
    {
        if (gameObject.CompareTag("Player"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (GetComponent<EnemyActor>())
            GetComponent<EnemyActor>().TurnInactive();
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<EnemyDissolve>().StartDissolve();
        deathParticle?.GetParticle(transform);
        GetComponentInChildren<Animator>().SetInteger("Var", Random.Range(0, 4));
        GetComponentInChildren<Animator>().SetTrigger("Death");
    }

    public void ReturnToMaxHP()
    {
        currentHP = maxHP;
        EventsManager.instance.InvokeHealthUpdateEvent(characterId, currentHP, 0, maxHP, gameObject);
    }
}
