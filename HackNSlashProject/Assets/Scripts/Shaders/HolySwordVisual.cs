using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class HolySwordVisual : MonoBehaviour
{
    [Header("Settings")]
    public string cutoffPropertyName;
    public float attackDuration;
    public float dissolveSpeed;
    public Vector2 minAndMaxValue;

    [Header("Particles")]
    public VisualEffect bladeParticle;
    public Vector3 forceValueCastOut;

    [Header("References")]
    public MeshRenderer swordMeshRenderer;

    private Material[] materials;
    private float height;
    
    private bool isPlaying;

    private void Start()
    {
        materials = swordMeshRenderer.materials;
        SetCutoffHeight(minAndMaxValue.x - 1);

        bladeParticle.Stop();
    }

    private void Update()
    {
        // if (Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     StartAttackVisual();
        // }
    }

    public void StartAttackVisual()
    {
        StartCoroutine(nameof(AttackVisualUp));    
    }
    
    public void StopAttackVisual()
    {
        StartCoroutine(nameof(AttackVisualDown));    
    }

    private IEnumerator AttackVisualUp()
    {
        StopCoroutine(nameof(AttackVisualDown));
        
        if(!isPlaying)
        {
            bladeParticle.Play();
            SetParticleForce(forceValueCastOut);
            isPlaying = true;
        }

        //show sword
        while (height < minAndMaxValue.y)
        {
            height = Mathf.MoveTowards(height, minAndMaxValue.y, dissolveSpeed * Time.deltaTime);
            SetCutoffHeight(height - 1); //to change 0:2 range to -1:1
            yield return null;
        }
    }
    
    private IEnumerator AttackVisualDown()
    {
        StopCoroutine(nameof(AttackVisualUp));
        
        //dissolve sword
        while (height > minAndMaxValue.x)
        {
            height = Mathf.MoveTowards(height, minAndMaxValue.x, dissolveSpeed * Time.deltaTime);
            SetCutoffHeight(height - 1); //to change 0:2 range to -1:1
            yield return null;
        }

        if(isPlaying)
        {
            bladeParticle.Stop();
            isPlaying = false;
        }
        
    }

    private void SetCutoffHeight(float value)
    {
        for (int i = 0; i < materials.Length; i++)
        {   
            materials[i].SetFloat(cutoffPropertyName, value);
        }
    }

    private void SetParticleForce(Vector3 value)
    {
        bladeParticle.SetVector3("ForceValue", value);
    }

}
