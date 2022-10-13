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
    
    private static readonly int StopTrigger = Animator.StringToHash("Stop");
    private static readonly int StartTrigger = Animator.StringToHash("Start");

    private void Start()
    {
        if(!activateOnStart)
            return;

        animator.SetTrigger(StartTrigger);
        StartCoroutine(nameof(StartDelay));
    }

    public void StopBell()
    {
        loop = false;
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delay);
        audio1.Play();
        yield return new WaitForSeconds(delay);
        audio2.Play();

        if(loop)
            StartCoroutine(nameof(StartDelay));
        else
        {
            animator.SetTrigger(StopTrigger);
        }
    }
}
