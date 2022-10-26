using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CleansingUIHandler : MonoBehaviour
{
    public float timeToCleanse = 4;
    public float activateDistance = 1;
    [Header("References")]
    public VisualEffect cleansingParticle;
    public GameObject player;
    public GameObject uiCanvas;
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
    }
    private void OnCleanseUpdateEvent(object sender, CleanseUpdateArgs e)
    {
        if(e.isCleanseCompleted)
            return;

        isCleansing = e.isCleansing;

        if (e.isCleansing)
        {
            timer = timeToCleanse;
        }
    }

    private void Update()
    {
        ChangeButtonUI();

        if (!isCleansing && !isComplete)
            return;

        if (player)
        {
            bool active = Vector3.Distance(gameObject.transform.position, player.transform.position) < activateDistance;
            uiCanvas.SetActive(active);
        }

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
                cleansingParticle.Play();

                EventsManager.instance.InvokeCleanseUpdateEvent(isCleansing, isComplete, this);
                break;
        }
    }

    private void ChangeButtonUI()
    {   
        //Check if input is from keyboard of controller
        if (InputChecker.usesController)
        {
            f_Key.gameObject.SetActive(false);
            buttonImage.gameObject.SetActive(true);

            if(buttonImage.gameObject.activeSelf)
                buttonImage.sprite = isCleansing ? b_Pressed : b_Normal;
        }
        else
        {
            buttonImage.gameObject.SetActive(false);
            f_Key.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        EventsManager.instance.CleanseUpdateEvent -= OnCleanseUpdateEvent;    
    }
}
