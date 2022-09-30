using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGameHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject pausePanel;

    [Header("Input")]
    public InputAction pauseInput;

    private bool isPaused = false;
    private bool settingsOpen = false;

    private void Update()
    {
        if (pauseInput.WasPressedThisFrame() && !settingsOpen)
            return;

        isPaused = !isPaused;

        HandlePauseMenu(isPaused, isPaused ? 1 : 0);
    }

    private void HandlePauseMenu(bool state, float time)
    {
        Time.timeScale = time;
        pausePanel.SetActive(state);
    }

    public void SetSettingsState(bool state)
    {
        settingsOpen = state;
    }
}
