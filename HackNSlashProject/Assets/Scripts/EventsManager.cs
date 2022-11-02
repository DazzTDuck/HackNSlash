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

public class CleanseUpdateArgs : EventArgs
{
    public bool isCleansing;
    public bool isCleanseCompleted;
}

public class InputSwitchArgs : EventArgs
{
    public bool usesController;
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

    public event EventHandler<CleanseUpdateArgs> CleanseUpdateEvent;
    public void InvokeCleanseUpdateEvent(bool isCleansing, bool isCleanseCompleted, object sender)
    {
        CleanseUpdateEvent?.Invoke(sender, new CleanseUpdateArgs{isCleansing = isCleansing, isCleanseCompleted = isCleanseCompleted});
    }

    public event EventHandler<InputSwitchArgs> InputSwitchEvent;
    public void InvokeInputSwitchEvent(object sender, bool usesController_)
    {
        InputSwitchEvent?.Invoke(sender, new InputSwitchArgs {usesController = usesController_ });
    }
}
