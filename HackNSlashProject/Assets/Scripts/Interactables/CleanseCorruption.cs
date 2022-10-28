using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseCorruption : Interactable
{
    public GameObject shine;
    bool hasCleansed;

    protected override void Start()
    {
        base.Start();
        EventsManager.instance.CleanseUpdateEvent += Cleanse;
    }
    private void OnDestroy()
    {
        EventsManager.instance.CleanseUpdateEvent -= Cleanse;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        bool b = false;
        if (!hasCleansed)
            b = CheckForLiveEnemies();
        if (b && !canInteract)
        {
            canInteract = true;
            shine.SetActive(true);
        }
        else if (!b && canInteract)
        {
            canInteract = false;
            shine.SetActive(false);
        }
    }
    bool CheckForLiveEnemies()
    {
        if (CombatManager.combatManager?.livingEnemies.Count == 0)
            return true;
        else return false;
    }
    public override void Interact(bool pressDown)
    {
        EventsManager.instance.InvokeCleanseUpdateEvent(pressDown, false, this);
    }

    void Cleanse(object sender, CleanseUpdateArgs cleanseUpdateArgs)
    {
        if (cleanseUpdateArgs.isCleanseCompleted)
            hasCleansed = true;
    }
}
