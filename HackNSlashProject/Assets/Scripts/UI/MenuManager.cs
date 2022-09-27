using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public float loadingDelay = 2f;
    public void LoadScene(int i)
    {
        StartCoroutine(SceneLoader(i, loadingDelay));
        //activate black screen
        LoadingScreenManager.instance.StartLoadingSequence(loadingDelay);
    }

    private IEnumerator SceneLoader(int i, float delay)
    {
        yield return new WaitForSeconds(delay);

        //load scene
        SceneManager.LoadScene(i);
    }
}
