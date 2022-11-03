using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class HolyWaterUIVisual : MonoBehaviour
{
    public PlayerHolyWater holyWater;
    [Space]
    public Image holyWaterImage;
    public Sprite[] sprites;
    
    private int currentIndex;
    private Animator animator;

    private void Start()
    {
        currentIndex = sprites.Length - 1;
        UpdateSprite();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!holyWater)
            return;
        
        if(currentIndex > holyWater.remainingUses)
        {
            //holy water has been used
            currentIndex = holyWater.remainingUses;
            animator.SetTrigger("Used");
            UpdateSprite();
        }        
    }
    
    public void ResetUI()
    {
        currentIndex = holyWater.remainingUses;
        UpdateSprite();    
    }
    
    private void UpdateSprite()
    {
        holyWaterImage.sprite = sprites[currentIndex];
    }

}
