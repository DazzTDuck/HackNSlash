using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputChecker : MonoBehaviour
{
    [Tooltip("Returns true if using a controller, or false if using keyboard & mouse")]
    public static bool usesController;

    /// <summary>
    /// Sets user mode to Keyboard & mouse
    /// </summary>
    public void UseKeyboardAndMouse()
    {
        if (usesController == true)
        {
            usesController = false;
            EventsManager.instance.InvokeInputSwitchEvent(this, false);
        }
    }
    /// <summary>
    /// Sets user mode to Controller
    /// </summary>
    public void UseGamepad()
    {
        if (usesController == false)
        {
            usesController = true;
            EventsManager.instance.InvokeInputSwitchEvent(this, true);
        }
    }
}
