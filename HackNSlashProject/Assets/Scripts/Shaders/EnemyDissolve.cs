using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;

public class EnemyDissolve : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Material dissolveTopMaterial;
    public Material dissolveBottomMaterial;
    public Material normalMaterial;
    public EnemyCorruption corruption;

    [Space]
    public float dissolveSpeed;
    public string propertyValue;
    public float dissolvedValue = -0.2f;
    public float unDissolvedValue = 0.2f;

    private bool dissolve;
    private void Update()
    {
        if (!dissolve)
            return;

        MaterialVariableLerpToNewValue(propertyValue, dissolvedValue, dissolveTopMaterial);
        MaterialVariableLerpToNewValue(propertyValue, dissolvedValue, dissolveBottomMaterial);
    }

    public void StartDissolve()
    {
        corruption.enabled = false;
        meshRenderer.material = dissolveTopMaterial;
        dissolve = true;
    }

    public void ResetMaterials()
    {
        dissolve = false;
        meshRenderer.material = normalMaterial;
        corruption.enabled = true;
        ResetValues();
    }

    public void MaterialVariableLerpToNewValue(string variableName, float newValue, Material material)
    {
        float currentValue = material.GetFloat(variableName); 
        float lerpValue = Mathf.Lerp(currentValue, newValue, Time.deltaTime * dissolveSpeed);
        material.SetFloat(variableName, lerpValue);

        if (Math.Abs(currentValue - newValue) < 0.01)
            dissolve = false;
    }

    private void ResetValues()
    {
        dissolveTopMaterial.SetFloat(propertyValue, unDissolvedValue);
        dissolveBottomMaterial.SetFloat(propertyValue, unDissolvedValue);
    }

    private void OnDisable()
    {
        ResetValues();    
    }
}
