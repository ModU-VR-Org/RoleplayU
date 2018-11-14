using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClickFromInspector : MonoBehaviour {

    public bool clickButton;
    public Button button;

	void Start ()
    {
        button = GetComponent<Button>();
	}

    // development tool function, allows you to click sign in button without a VR headset by using the inspector 
    private void Update()
    {
        if (clickButton)
        {
            button.onClick.Invoke();
            clickButton = false;
        }
    }
}
