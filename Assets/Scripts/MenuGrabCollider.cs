using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGrabCollider : MonoBehaviour, IGrabbable
{
    public Transform menuOffsetLeft;
    public Transform menuOffsetRight;
    public GameObject menu;

    public bool Grab(GrabbingTool.GrabbingHand hand)
    {
        menu.SetActive(true);

        if (hand.left)
        {
            menu.transform.position = menuOffsetLeft.position;
            menu.transform.rotation = menuOffsetLeft.rotation;
        }
        else
        {
            menu.transform.position = menuOffsetRight.position;
            menu.transform.rotation = menuOffsetRight.rotation;
        }

        menu.GetComponentInChildren<GrabbableMenu>().Grab(hand);

        return true;
    }

    public void UnGrab()
    {
        // do nothing
    }
}
