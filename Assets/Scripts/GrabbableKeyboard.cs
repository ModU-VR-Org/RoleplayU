using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableKeyboard : MonoBehaviour, IGrabbable
{
    public Transform keyboardRoot;

    public Transform defaultParent;

    private GrabbableMenu grabbableMenu;

    private void Start()
    {
        defaultParent = keyboardRoot.parent;
        grabbableMenu = transform.root.gameObject.GetComponentInChildren<GrabbableMenu>();
    }

    public bool Grab(GrabbingTool.GrabbingHand hand)
    {
        keyboardRoot.SetParent(hand.controllerTransform);
        return true;
    }

    public void UnGrab()
    {
        ReattatchKeyboard();
    }

    private void DettatchKeyboard()
    {
        keyboardRoot.SetParent(null);
    }

    public void ReattatchKeyboard()
    {
        keyboardRoot.SetParent(defaultParent);
    }

    private void OnEnable()
    {
        if (grabbableMenu != null)
        {
            grabbableMenu.OnMenuGrab += DettatchKeyboard;
            grabbableMenu.OnMenuUnGrab += ReattatchKeyboard;
        }
    }

    private void OnDisable()
    {
        if (grabbableMenu != null)
        {
            grabbableMenu.OnMenuGrab -= DettatchKeyboard;
            grabbableMenu.OnMenuGrab -= ReattatchKeyboard;
        }
    }
}
