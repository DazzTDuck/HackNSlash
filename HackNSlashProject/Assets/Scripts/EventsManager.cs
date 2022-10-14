using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthUpdateArgs : EventArgs
{
    public int id;
    public int currentAmount;
    public int minAmount;
    public int maxAmount;
}

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event EventHandler<HealthUpdateArgs> HealthUpdateEvent;
    public void InvokeHealthUpdateEvent(int id, int currentAmount, int minAmount, int maxAmount, object sender)
    {
        HealthUpdateEvent?.Invoke(sender, new HealthUpdateArgs{id = id, currentAmount = currentAmount, minAmount = minAmount, maxAmount = maxAmount});
    }

    // public event Action<int> onDeath;
    // public void OnDeath(int id)
    // {
    //     onDeath?.Invoke(id);
    // }
}
