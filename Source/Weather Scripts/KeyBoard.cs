using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour
{
    private TouchScreenKeyboard _keyboard; 

    public void OpenKeyboard()
    {
        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }

    [System.Obsolete]
    private void Update()
    {
        if (TouchScreenKeyboard.visible == false && _keyboard != null)
        {
            if (_keyboard.done)
            {
                _keyboard = null;
            }
        }
    }
}
