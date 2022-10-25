using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CleansingUIHandler : MonoBehaviour
{
    public float timeToCleanse = 4;
    [Header("References")]
    public Image progressImage;
    public Image buttonImage;
    public TMP_Text f_Key;
    [Header("Sprites")]
    public Sprite b_Pressed;
    public Sprite b_Normal;

    private float timer;
    private bool isCleansing;
    private bool isComplete;

    private void Start()
    {
        EventsManager.instance.CleanseUpdateEvent += OnCleanseUpdateEvent;
        isCleansing = true;
        timer = timeToCleanse;
    }
    private void OnCleanseUpdateEvent(object sender, CleanseUpdateArgs e)
    {
        if(e.isCleanseCompleted)
            return;

        isCleansing = e.isCleansing;
    }

    private void Update()
    {
        ChangeButtonUI();

        if (!isCleansing && !isComplete)
            return;

        switch (timer)
        {
            case > 0:
                timer -= Time.deltaTime;

                //update radial progress UI (1 - to invert the number to up)
                progressImage.fillAmount = 1 - (float)HealthbarHandler.GetPercentage01(timer, 0, timeToCleanse);
                break;
            case <= 0:
                //cleansing is complete
                isCleansing = false;
                isComplete = true;

                EventsManager.instance.InvokeCleanseUpdateEvent(isCleansing, isComplete, this);
                break;
        }
    }

    private void ChangeButtonUI()
    {   
        //Check if input is from keyboard of controller

        if(buttonImage.IsActive())
            buttonImage.sprite = isCleansing ? b_Pressed : b_Normal;
    }

    private void OnDisable()
    {
        EventsManager.instance.CleanseUpdateEvent -= OnCleanseUpdateEvent;    
    }
}
