using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarHandler : MonoBehaviour
{
    public float reductionSpeed = 0.5f;
    public float reductionDelay = 1.5f;
    
    [Header("References")]
    public SlicedFilledImage mainBar;
    public SlicedFilledImage reductionBar;

    private double currentPercentage;
    private double reductionBarPercentage;
    private float newValue;
    private float maxValue;
    private float minValue;

    private void Start()
    {
        UpdateBar(50, 0, 100);
    }

    public void UpdateBar(float newValue, float minValue, float maxValue)
    {
        //to make sure the value never goes below the minimum
        if(newValue < minValue)
            newValue = minValue;
        
        this.newValue = newValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
        
        currentPercentage = GetPercentage01(newValue, minValue, maxValue);
        mainBar.fillAmount = (float)currentPercentage;
        
        StartCoroutine(nameof(UpdateDelay));
    }
    
    private IEnumerator UpdateDelay()
    {
        yield return new WaitForSeconds(reductionDelay);
        
        //change this later to lerp to the current value; 
        reductionBarPercentage = GetPercentage01(newValue, minValue, maxValue);
        reductionBar.fillAmount = (float)reductionBarPercentage;
    }
    
    private double GetPercentage01(float input, float min, float max)
    {
        //(currentValue - minValue) / (maxValue - minValue);
        double value = (input - min) / (max - min);
        //rounds off to 3 digits
        return Math.Round(value, 3); 
    }
}
