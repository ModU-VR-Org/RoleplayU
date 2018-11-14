using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ModelInteraction_NET : MonoBehaviour, ISelectable, IGrabbable
{
    private bool isSelected;
    private bool isGrabbed;

    public bool Select()
    {
        if (isGrabbed == false)
        {
            GetComponent<PhotonView>().RequestOwnership();
            GetComponent<PhotonView>().RPC("ClaimSelection", RpcTarget.Others);
            GetComponent<SelectionIndicator>().ToggleHighlight(true);
            isSelected = true;
            return true;
        }
        return false;
    }

    public void Deselect()
    {
        GetComponent<SelectionIndicator>().ToggleHighlight(false);
        isSelected = false;
    }

    public bool Grab(GrabbingTool.GrabbingHand hand)
    {
        if (GetComponent<PhotonView>().IsMine && isSelected)
        {
            GetComponent<PhotonView>().RPC("SyncGrab", RpcTarget.All, true);
            transform.SetParent(hand.controllerTransform);
            return true;
        }
        return false;
    }

    public void UnGrab()
    {
        GetComponent<PhotonView>().RPC("SyncGrab", RpcTarget.All, false);
        transform.SetParent(null);
    }

    [PunRPC]
    public void SyncGrab(bool grabbedState)
    {
        isGrabbed = grabbedState;
    }

    [PunRPC]
    public void ClaimSelection()
    {
        Deselect();
    }
}
