using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseCorruption : Interactable
{
    public GameObject shine;
    bool hasCleansed;

    private void FixedUpdate()
    {
        bool b = false;
        if (!hasCleansed)
            b = CheckForLiveEnemies();
        if (b && !canInteract)
        {
            canInteract = true;
            shine.SetActive(true);
        }
        else if (!b)
        {
            hasCleansed = true;
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
}
