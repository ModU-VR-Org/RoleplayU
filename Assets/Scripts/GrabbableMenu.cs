using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableMenu : MonoBehaviour, IGrabbable
{
    public Transform menuRoot;
    public delegate void KeyboardGrabHandler();
    public event KeyboardGrabHandler OnMenuGrab;
    public event KeyboardGrabHandler OnMenuUnGrab;

    private bool isMenuHeld;

    public bool Grab(GrabbingTool.GrabbingHand hand)
    {
        isMenuHeld = true;
        menuRoot.SetParent(hand.controllerTransform);

        if (OnMenuGrab != null)
            OnMenuGrab();

        return true;
    }

    public void UnGrab()
    {
        menuRoot.SetParent(null);

        if (OnMenuUnGrab != null)
            OnMenuUnGrab();
    }
}
