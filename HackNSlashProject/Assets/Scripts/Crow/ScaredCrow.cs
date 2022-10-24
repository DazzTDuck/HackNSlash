using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ScaredCrow : MonoBehaviour
{
    Animator animator;
    public Quaternion flyDir;
    public float flySpeed;

    Quaternion currentDir;
    bool inFlight;
    Rigidbody rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentDir = transform.rotation;
        currentDir.y += 0.1f;
        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        while (!inFlight)
        {
            int anim = Random.Range(0, 2);
            animator.SetTrigger(anim == 1 ? "Scratch" : "Stretch");
            yield return new WaitForSeconds(Random.Range(2.5f, 5f));
        }
    }

    public void FlyAway()
    {
        if (!inFlight)
        {
            StartCoroutine(Flying());
            inFlight = true;
        }
    }

    IEnumerator Flying()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 1.5f));
        animator.SetTrigger("Fly");
        yield return new WaitForSeconds(0.3f);
        float time = 0;
        while (time < 15)
        {
            rb.MovePosition(transform.position + transform.forward * flySpeed * Time.fixedDeltaTime);
            currentDir = Quaternion.RotateTowards(currentDir, flyDir, flySpeed * 3);
            time += Time.fixedDeltaTime;
            transform.rotation = currentDir;
            if (time < 3)
                flySpeed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }
}
