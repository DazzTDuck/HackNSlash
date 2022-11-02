using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CorruptionState
{
    public float noiseScale = 15;
    public float noiseIntensity = 1.69f;
    public float noiseScrollSpeed = 0.1f;
}

public class EnemyCorruption : MonoBehaviour
{
    public Material corruptionMaterial;
    public EnemyActor enemyActor;
    public float changeStateSpeed = 5;
    [Space]
    public CorruptionState patrolState;
    public CorruptionState engageState;
    private void Update()
    {
        if(!enemyActor)
            return;

        switch (enemyActor.state)
        {
            case Enemystates.Partoling:
                LerpToState(patrolState);
                break;
            case Enemystates.Engaged:
                LerpToState(engageState);
                break;
        }
    }

    public void LerpToState(CorruptionState newState)
    {
        if(Math.Abs(corruptionMaterial.GetFloat("_NoiseScale") - newState.noiseScale) > 0)
            MaterialVariableLerpToNewValue("_NoiseScale", newState.noiseScale);

        if(Math.Abs(corruptionMaterial.GetFloat("_NoiseIntensity") - newState.noiseIntensity) > 0)
            MaterialVariableLerpToNewValue("_NoiseIntensity", newState.noiseIntensity);

        if(Math.Abs(corruptionMaterial.GetFloat("_NoiseScrollSpeed") - newState.noiseScrollSpeed) > 0)
            MaterialVariableLerpToNewValue("_NoiseScrollSpeed", newState.noiseScrollSpeed);
    }

    public void MaterialVariableLerpToNewValue(string variableName, float newValue)
    {
        float currentValue = corruptionMaterial.GetFloat(variableName); 
        float lerpValue = Mathf.Lerp(currentValue, newValue, Time.deltaTime * changeStateSpeed);
        corruptionMaterial.SetFloat(variableName, lerpValue);
    }
}
