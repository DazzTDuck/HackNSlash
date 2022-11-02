using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSounds : MonoBehaviour
{
    public string soundName;

    public float footstep1Timing = 0.1f;
    public float footstep2Timing = 0.2f;

    private bool running;

    public void StartRunning()
    {
        StartCoroutine(nameof(PlayingSounds));
        running = true;
    }

    public void StopRunning()
    {
        running = false;    
    }

    private IEnumerator PlayingSounds()
    {
        yield return new WaitForSeconds(footstep1Timing);
        
        if(AudioManager.instance)
           AudioManager.instance.PlaySound(soundName); 
                                            
        yield return new WaitForSeconds(footstep2Timing);

        if(AudioManager.instance)
           AudioManager.instance.PlaySound(soundName);

        if (running)
        {
            StartCoroutine(nameof(PlayingSounds));
        }
    }
}
