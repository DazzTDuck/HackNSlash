using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void OpenSettings()
    {
        
    }

    public void LoadScene(int i)
    {
        StartCoroutine(SceneLoader(i, 2f));
        //activate black screen
        StartCoroutine(LoadingScreenManager.instance.StartLoadingSequence(2f));
    }

    private IEnumerator SceneLoader(int i, float delay)
    {
        yield return new WaitForSeconds(delay);

        //load scene
        SceneManager.LoadScene(i);
    }
}
