using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseCorruption : Interactable
{
    public GameObject shine;
    bool hasCleansed;
    private void Start()
    {
        StartCoroutine(RegUpdates());
    }
    IEnumerator RegUpdates()
    {
        while (!canInteract && !hasCleansed)
        {
            yield return new WaitForSeconds(0.1f);
            CheckForLiveEnemies();
        }
    }
    void CheckForLiveEnemies()
    {
        if (CombatManager.combatManager?.livingEnemies.Count == 0)
        {
            canInteract = true;
            shine.SetActive(true);
            Debug.Log("Thee shall cleanse the corruption");
        }
    }
    public override void Interact()
    {
        Debug.Log("Thee hath cleansed the corruption");
        hasCleansed = true;
        canInteract = false;
        shine.SetActive(false);
    }
}
