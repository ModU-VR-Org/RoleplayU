using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRKeys;

public class KeyboardInputFieldSetter : MonoBehaviour
{
    public Keyboard keyboard;
    public TMP_InputField inputField;
    public bool selected;

    private void Start()
    {
        inputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return keyboard.MyValidate(addedChar); };
    }

    private void Update()
    {
        if (selected == false && inputField.isFocused)
        {
            selected = true;
            keyboard.displayText = inputField;
        }
        else if (selected == true && inputField.isFocused == false)
        {
            selected = false;
        }
    }
}
