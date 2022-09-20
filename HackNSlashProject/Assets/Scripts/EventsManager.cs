using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnDamageArgs : EventArgs
{
    public int currentAmount;
    public int minAmount;
    public int maxAmount;
    public GameObject objectFrom;
}

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event EventHandler<OnDamageArgs> OnDamageEvent;
    public void InvokeOnDamageEvent(int currentAmount, int minAmount, int maxAmount, GameObject objectFrom)
    {
        OnDamageEvent?.Invoke(this, new OnDamageArgs{currentAmount = currentAmount, minAmount = minAmount, maxAmount = maxAmount, objectFrom = objectFrom});
    }

    // public event Action<int> onDeath;
    // public void OnDeath(int id)
    // {
    //     onDeath?.Invoke(id);
    // }
}
