using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallAllCrows : MonoBehaviour
{
    ScaredCrow[] crows;
    private void Awake()
    {
        crows = FindObjectsOfType<ScaredCrow>();
    }
    public void CallCrows()
    {
        foreach(ScaredCrow crow in crows)
        {
            crow.FlyAway();
        }
    }
}
