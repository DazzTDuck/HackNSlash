using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    public string tagToCheck;
    [Space]
    public UnityEvent onTriggerEnter;

    private bool hasTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if(hasTriggered)
            return;

        if (other.CompareTag(tagToCheck))
        {
            onTriggerEnter?.Invoke();
            hasTriggered = true;
        }
    }
}
