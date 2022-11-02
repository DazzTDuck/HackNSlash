using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int stage;
    public GameObject[] kbmTutorials;
    public GameObject[] cTutorials;
    public GameObject[] tutorialStage;
    public EnemyActor[] newEnemies;
    public GameObject[] cleanseGame;

    private void Start()
    {
        EventsManager.instance.InputSwitchEvent += SwitchInput;
    }

    void SwitchInput(object sender, InputSwitchArgs e)
    {
        if (e.usesController == true)
        {
            for (int i = 0; i < kbmTutorials.Length; i++)
            {
                kbmTutorials[i].SetActive(false);
            }
            for (int i = 0; i < cTutorials.Length; i++)
            {
                cTutorials[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < kbmTutorials.Length; i++)
            {
                kbmTutorials[i].SetActive(true);
            }
            for (int i = 0; i < cTutorials.Length; i++)
            {
                cTutorials[i].SetActive(false);
            }
        }
    }

    public void SwitchTutorialStage(int newStage)
    {
        tutorialStage[stage].SetActive(false);
        tutorialStage[newStage].SetActive(true);
        stage = newStage;
    }

    void CheckCleanse(object sender, CleanseUpdateArgs e)
    {
        if (e.isCleanseCompleted == true)
            Invoke("FinishTutorial", 1f);
    }
    void FinishTutorial()
    {
        CombatManager.combatManager.livingEnemies.Clear();
        CombatManager.combatManager.deadEnemies.Clear();
        for (int i = 0; i < newEnemies.Length; i++)
        {
            CombatManager.combatManager.livingEnemies.Add(newEnemies[i]);
            newEnemies[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < cleanseGame.Length; i++)
        {
            cleanseGame[i].SetActive(true);
        }
        FindObjectOfType<PlayerHolyWater>().remainingUses = FindObjectOfType<PlayerHolyWater>().maxUses;
        gameObject.SetActive(false);
    }
}
