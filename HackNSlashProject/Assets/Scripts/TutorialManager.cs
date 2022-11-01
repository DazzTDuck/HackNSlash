using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int stage;
    public GameObject[] kbmTutorials;
    public GameObject[] cTutorials;
    public GameObject[] tutorialStage;

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
}
