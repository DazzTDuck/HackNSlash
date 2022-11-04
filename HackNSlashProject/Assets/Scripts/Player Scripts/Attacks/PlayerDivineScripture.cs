using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDivineScripture : MonoBehaviour
{
    public float knockBack;
    public float duration;
    public float delay;
    public float lockoutAfter;
    public LayerMask enemyLayer;
    public float radius;
    [Range(0, 90)]
    public float angle;
    public int damage;

    public float qTime;
    public Animator animator;
    bool attackQ;
    public int powerConsumption;
    PlayerMovement player;

    public Transform particleOrigin;
    public ParticleManager particleManager;


    public void ReadScripture(PlayerMovement player_)
    {
        player = player_;
        if (!attackQ)
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
                StartCoroutine(Begone());
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        attackQ = false;
    }

    IEnumerator Begone()
    {
        player.canAct = false;
        animator.SetTrigger("Book");
        AudioManager.instance.PlaySound("HolyBook2");
        yield return new WaitForSeconds(delay);
        AudioManager.instance.PlaySound("HolyBook1");
        particleManager.GetParticle(particleOrigin);
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (Collider enemyColider in enemies)
        {
            float angle_ = Vector3.Angle(enemyColider.transform.position - transform.position, transform.forward);
            if (angle_ < angle /2)
            {
                enemyColider.GetComponentInParent<ActorStunned>()?.GetStunned(duration, knockBack, transform);
                int i = Random.Range(-5, 5);
                enemyColider.GetComponentInParent<CharacterHealth>()?.TakeDamage(damage + i);
            }
        }
        yield return new WaitForSeconds(lockoutAfter);
        player.canAct = true;
    }

    private void FixedUpdate()
    {
        Vector2 tri = new Vector2();
        tri.x = Mathf.Sin(angle / Mathf.Rad2Deg / 2) * radius;
        tri.y = Mathf.Cos(angle / Mathf.Rad2Deg / 2) * radius;
        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(-tri.x, 0, tri.y)), Color.blue, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(+tri.x, 0, tri.y)), Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + transform.forward * radius, Color.black, Time.fixedDeltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
