using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;

public class CleansingUIHandler : MonoBehaviour
{
    public float timeToCleanse = 4;
    public float activateDistance = 1;
    [Header("References")]
    public GameObject player;
    public GameObject uiCanvas;
    public Image progressImage;
    public Image buttonImage;
    [Header("Sprites")]
    public Sprite b_Pressed;
    public Sprite b_Normal;
    public Sprite f_Key;
    
    [Space]
    public UnityEvent onCleansed;

    private float timer;
    private bool isCleansing;
    private bool isComplete;
    CleanseCorruption handler;

    private void Start()
    {
        EventsManager.instance.CleanseUpdateEvent += OnCleanseUpdateEvent;
        uiCanvas.SetActive(false);
        handler = GetComponentInParent<CleanseCorruption>();
    }
    private void OnCleanseUpdateEvent(object sender, CleanseUpdateArgs e)
    {
        if(e.isCleanseCompleted)
            return;

        isCleansing = e.isCleansing;

        if (e.isCleansing)
        {
            timer = timeToCleanse;
            StartCoroutine(nameof(CleanseSound));
        }
        else
        {
            if(progressImage.fillAmount != 0)
                progressImage.fillAmount = 0;   
            
            StopCoroutine(nameof(CleanseSound));
        }
    }

    private void Update()
    {
        ChangeButtonUI();
        
        if (player && handler.canInteract)
        {
            bool active = Vector3.Distance(gameObject.transform.position, player.transform.position) < activateDistance;
            uiCanvas.SetActive(active);
        }

        if (!isCleansing || isComplete)
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
                uiCanvas.SetActive(false);
                AudioManager.instance.PlaySound("ObjectiveComplete");

                EventsManager.instance.InvokeCleanseUpdateEvent(false, isComplete, this);
                onCleansed?.Invoke();
                Debug.Log("Completed");
                break;
        }
    }
    
    private IEnumerator CleanseSound()
    {
        yield return new WaitForSeconds(0.6f);
        AudioManager.instance.PlaySound("Cleanse");
    }

    private void ChangeButtonUI()
    {   
        //Check if input is from keyboard of controller
        if (InputChecker.usesController)
            buttonImage.sprite = isCleansing ? b_Pressed : b_Normal; 
        else
            buttonImage.sprite = f_Key;
    }

    private void OnDisable()
    {
        EventsManager.instance.CleanseUpdateEvent -= OnCleanseUpdateEvent;    
    }
}
