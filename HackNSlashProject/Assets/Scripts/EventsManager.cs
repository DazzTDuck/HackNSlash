using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnBarArgs : EventArgs
{
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

    public event EventHandler<OnBarArgs> OnBarUpdateEvent;
    public void InvokeOnBarEvent(int currentAmount, int minAmount, int maxAmount, object sender)
    {
        OnBarUpdateEvent?.Invoke(sender, new OnBarArgs{currentAmount = currentAmount, minAmount = minAmount, maxAmount = maxAmount});
    }

    // public event Action<int> onDeath;
    // public void OnDeath(int id)
    // {
    //     onDeath?.Invoke(id);
    // }
}
