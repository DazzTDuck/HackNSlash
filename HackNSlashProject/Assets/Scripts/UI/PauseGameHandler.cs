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

    private bool usingController = true;

    private void Start()
    {
        pauseInput.Enable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void Update()
    {
        if(pauseInput.WasPressedThisFrame())
        {
            SwitchPauseMenu();
        }

        if (!usingController && isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        usingController = inputDevice == Mouse.current; 
    }

    public void SwitchPauseMenu()
    {
        SetFirstSelectedPauseMenu();
        
        isPaused = !isPaused;

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

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
