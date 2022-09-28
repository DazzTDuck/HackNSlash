using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValue : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textToChange;

    private void Start()
    {
        OnSliderChange(slider.value);
    }

    public void OnSliderChange(float value)
    {
        var percentage01 = HealthbarHandler.GetPercentage01(value, slider.minValue, slider.maxValue) * 100;
        int percentage = Mathf.RoundToInt((float)percentage01);
        textToChange.text = percentage.ToString();
    }
}
