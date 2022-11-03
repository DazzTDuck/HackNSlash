using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunningSounds : MonoBehaviour
{
    public string soundName;
    public EnemySoundManager soundManager;

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
           soundManager.PlaySound(soundName); 
                                            
        yield return new WaitForSeconds(footstep2Timing);

        if(AudioManager.instance)
           soundManager.PlaySound(soundName);

        if (running)
        {
            StartCoroutine(nameof(PlayingSounds));
        }
    }
}
