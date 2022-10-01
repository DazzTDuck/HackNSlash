using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseGameHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject firstButtonPauseMenu;
    public GameObject firstButtonSettingsMenu;
    public EventSystem eventSystem;

    [Header("Input")]
    public InputAction pauseInput;

    public static bool isPaused = false;

    private void Start()
    {
        pauseInput.Enable();
    }

    private void Update()
    {
        if(pauseInput.WasPressedThisFrame())
        {
            SwitchPauseMenu();
        }
    }
    
    public void SwitchPauseMenu()
    {
        SetFirstSelectedPauseMenu();
        
        isPaused = !isPaused;
        
        settingsPanel.SetActive(false);

        HandlePauseMenu(isPaused, isPaused ? 0 : 1);    
    }

    private void HandlePauseMenu(bool state, float time)
    {
        Time.timeScale = time;
        pausePanel.SetActive(state);
    }
    
    public void SetFirstSelectedSettingsMenu()
    {
        eventSystem.SetSelectedGameObject(firstButtonSettingsMenu);
    }
    public void SetFirstSelectedPauseMenu()
    {
        eventSystem.SetSelectedGameObject(firstButtonPauseMenu);     
    }
    
    public void LoadMainMenu()
    {
        SwitchPauseMenu();
        LoadingScreenManager.instance.StartLoadingSequence(2f, 0);
    }
}
