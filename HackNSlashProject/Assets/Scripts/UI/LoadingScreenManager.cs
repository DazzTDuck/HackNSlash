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

    public IEnumerator StartLoadingSequence(float delay)
    {
        //fade in
        while (loadingPanel.color.a < 0.99f)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, fadeInColour, fadeSpeed * Time.deltaTime);
            yield return false;
        }

        loadingPanel.color = fadeInColour;

        yield return new WaitForSeconds(delay);

        //fade to black
        while (loadingPanel.color.r < 0.99f)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, fadeOutColour, fadeSpeed * Time.deltaTime);
            yield return false;
        }

        yield return new WaitForSeconds(delay);

        loadingPanel.color = fadeOutColour;

        //fade out
        while (loadingPanel.color.a > 0.05)
        {
            loadingPanel.color = Color.Lerp(loadingPanel.color, ColourReset, fadeSpeed * Time.deltaTime);
            yield return false;
        }
        loadingPanel.color = ColourReset;
    }
}
