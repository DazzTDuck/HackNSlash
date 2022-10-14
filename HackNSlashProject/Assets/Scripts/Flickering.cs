using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Flickering : MonoBehaviour
{
    public Light _lightSource;
    [Space]
    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;

    private float _baseIntensity;
    private bool _flickering;

    public void Reset()
    {
        MaxReduction = 0.2f;
        MaxIncrease = 0.2f;
        RateDamping = 0.1f;
        Strength = 300;
    }

    public void Start()
    {
        _baseIntensity = _lightSource.intensity;
        StartCoroutine(DoFlicker());
    }

    void Update()
    {
        if (!StopFlickering && !_flickering)
        {
            StartCoroutine(DoFlicker());
        }
    }

    private IEnumerator DoFlicker()
    {
        _flickering = true;
        while (!StopFlickering)
        {
            _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
        _flickering = false;
    }
}