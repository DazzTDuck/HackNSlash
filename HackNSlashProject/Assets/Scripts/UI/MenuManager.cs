using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public float loadingDelay = 2f;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(int i)
    {
        //activate black screen
        LoadingScreenManager.instance.StartLoadingSequence(loadingDelay, i);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
