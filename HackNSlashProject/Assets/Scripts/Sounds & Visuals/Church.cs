using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : MonoBehaviour
{
    public bool activateOnStart;
    public Animator animator;
    [Space]
    public float delay;
    public AudioSource audio1;
    public AudioSource audio2;

    private bool loop = true;

    private void Start()
    {
        if(!activateOnStart)
            return;

        animator.SetTrigger("Start");
        StartCoroutine(nameof(StartDelay));
    }

    public void StopBell()
    {
        loop = false;
        audio1.Stop();
        audio2.Stop();
        animator.SetTrigger("Stop");
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delay);
        audio1.Play();
        yield return new WaitForSeconds(delay);
        audio2.Play();

        if(loop)
            StartCoroutine(nameof(StartDelay));
    }
}
