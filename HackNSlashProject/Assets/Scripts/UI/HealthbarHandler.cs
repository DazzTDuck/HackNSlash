using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthbarHandler : MonoBehaviour
{
    public float reductionSpeed = 1f;
    public float reductionDelay = 1.25f;

    [Header("Fade")]
    public bool fadeInAndOut = false;
    public float fadeSpeed;
    public CanvasGroup canvasGroup;

    [Header("References")]
    public SlicedFilledImage mainBar;
    public SlicedFilledImage reductionBar;
    public TMP_Text damageAmountText;

    private double currentPercentage;
    private double reductionBarPercentage;
    private int newValue = 100;
    private int maxValue;
    private int minValue;
    private int lastHealthAmount;
    private int damageDone = 0;

    private void Awake()
    {
        HealthbarAlpha(0f);
    }

    private void Start()
    {
        EventsManager.instance.OnDamageEvent += OnDamageEvent;
    }
    private void OnDamageEvent(object sender, OnDamageArgs e)
    {
        if(e.objectFrom == gameObject)
            UpdateBar(e.currentAmount, e.minAmount, e.maxAmount);

        Debug.Log(e.objectFrom);
    }

    public void UpdateBar(int newValue, int minValue, int maxValue)
    {
        //to make sure the value never goes below the minimum
        if(newValue < minValue)
            newValue = minValue;
        
        this.newValue = newValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        
        //fade in healthbar
        HealthbarAlpha(1f);

        currentPercentage = GetPercentage01(newValue, minValue, maxValue);
        mainBar.fillAmount = (float)currentPercentage;
        
        //this will only activate at the beginning so every value is set correctly 
        if (reductionBarPercentage <= 0)
        {
            reductionBarPercentage = GetPercentage01(this.maxValue, minValue, maxValue);
            lastHealthAmount = this.maxValue;    
        }

        damageDone = lastHealthAmount - newValue;

        if (damageAmountText)
            damageAmountText.text = damageDone.ToString();

        StopCoroutine(nameof(UpdateDelay));
        StartCoroutine(nameof(UpdateDelay));
    }
    
    private IEnumerator UpdateDelay()
    {
        yield return new WaitForSeconds(reductionDelay);

            //set text to nothing so you cannot see it anymore and reset damage done
            if (damageAmountText)
                damageAmountText.text = "";

            damageDone = 0;
            lastHealthAmount = newValue;

        while (Math.Abs(reductionBarPercentage - currentPercentage) > 0)
        {
            reductionBarPercentage = Mathf.MoveTowards((float)reductionBarPercentage, (float)currentPercentage, Time.deltaTime * reductionSpeed);
            reductionBar.fillAmount = (float)reductionBarPercentage;

            yield return null;
        }

        //wait the same delay for the healthbar to disappear
        yield return new WaitForSeconds(reductionDelay);

        //if healthbar is empty after it is all reduced, set alpha to 0.
        if(currentPercentage >= 0)
            HealthbarAlpha(0);
        
        //make sure the Coroutine stopped
        StopCoroutine(nameof(UpdateDelay));
    }

    private void HealthbarAlpha(float alpha)
    {
        if(fadeInAndOut)
            StartCoroutine(FadeInOrOut(alpha));
    }
    private IEnumerator FadeInOrOut(float valueTo)
    {
        while (Math.Abs(canvasGroup.alpha - valueTo) > 0)
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, valueTo, Time.deltaTime * fadeSpeed);
            yield return null;    
        }
    }

    private static double GetPercentage01(int input, int min, int max)
    {
        double value = (float)(input - min) / (max - min);
        //rounds off to 3 digits
        return Math.Round(value, 3); 
    }
}
