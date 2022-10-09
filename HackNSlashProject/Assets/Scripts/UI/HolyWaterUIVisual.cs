using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HolyWaterUIVisual : MonoBehaviour
{
    public PlayerHolyWater holyWater;
    [Space]
    public Image holyWaterImage;
    public Sprite[] sprites;
    
    private int currentIndex;

    private void Start()
    {
        currentIndex = sprites.Length - 1;
        UpdateSprite();
    }

    private void Update()
    {
        if(!holyWater)
            return;
        
        if(currentIndex > holyWater.remainingUses)
        {
            //holy water has been used
            currentIndex = holyWater.remainingUses;
            
            UpdateSprite();
        }        
    }
    
    private void UpdateSprite()
    {
        holyWaterImage.sprite = sprites[currentIndex];
    }

}
