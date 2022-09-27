using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;

    public Image loadingPanel;
    public float fadeSpeed = 5;
    public float fadeDifference = 0.99f;
    public Color fadeInColour;
    public Color fadeOutColour;
    public Color ColourReset;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    
    public void StartLoadingSequence(float delay)
    {
        StartCoroutine(LoadingSequence(delay));    
    }
    
    private IEnumerator LoadingSequence(float delay)
    {
        //fade in
        while (loadingPanel.color.a < fadeDifference)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, fadeInColour, fadeSpeed * Time.deltaTime);
            yield return false;
        }

        loadingPanel.color = fadeInColour;

        yield return new WaitForSeconds(delay / 2);

        //fade to black
        while (loadingPanel.color.r < fadeDifference)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, fadeOutColour, fadeSpeed * Time.deltaTime);
            yield return false;
        }

        loadingPanel.color = fadeOutColour;
        
        yield return new WaitForSeconds(delay / 2);

        //fade out
        while (loadingPanel.color.a > 0.05f)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, ColourReset, fadeSpeed * Time.deltaTime);
            yield return false;
        }
        loadingPanel.color = ColourReset;
    }
}
