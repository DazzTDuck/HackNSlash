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
    public VisualEffect[] fullBladeParticles;

    [Header("References")]
    public MeshRenderer swordMeshRenderer;

    private Material[] materials;
    private float height;

    private void Start()
    {
        materials = swordMeshRenderer.materials;
        SetCutoffHeight(minAndMaxValue.x - 1);

        SetFullParticles(false);
        //StartAttackVisual();
    }

    public void StartAttackVisual()
    {
        StartCoroutine(nameof(AttackVisual));    
    }

    private IEnumerator AttackVisual()
    {
        height = 0;
        yield return new WaitForSeconds(3f); //debug delay

        Debug.Log("Starting Visual");

        SetFullParticles(true);

        //show sword
        while (Math.Abs(height - minAndMaxValue.y) > 0)
        {
            height = Mathf.MoveTowards(height, minAndMaxValue.y, dissolveSpeed * Time.deltaTime);
            SetCutoffHeight(height - 1); //to change 0:2 range to -1:1
            yield return null;
        }

        yield return new WaitForSeconds(attackDuration / 2);

        //dissolve sword
        while (Math.Abs(height - minAndMaxValue.x) > 0)
        {
            height = Mathf.MoveTowards(height, minAndMaxValue.x, dissolveSpeed * Time.deltaTime);
            SetCutoffHeight(height - 1); //to change 0:2 range to -1:1
            yield return null;
        }

        SetFullParticles(false);
    }

    private void SetCutoffHeight(float value)
    {
        for (int i = 0; i < materials.Length; i++)
        {   
            materials[i].SetFloat(cutoffPropertyName, value);
        }
    }

    private void SetFullParticles(bool value)
    {
        foreach (var particle in fullBladeParticles)
        {
            if(value)
                particle.Play();
            else
                particle.Stop();
        }
    }
}
