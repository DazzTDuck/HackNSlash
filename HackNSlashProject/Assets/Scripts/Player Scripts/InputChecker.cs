using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    public static bool usesController;

    public void UseKeyboardAndMouse()
    {
        usesController = false;
        Debug.Log("Keyboard & Mouse");
    }
    public void UseGamepad()
    {
        usesController = true;
        Debug.Log("controller");
    }
}
