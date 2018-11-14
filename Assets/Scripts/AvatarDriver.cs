using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class AvatarDriver : MonoBehaviourPun
{
    public Transform avatarHeadTransform;
    public Transform avatarHandTransformLeft;
    public Transform avatarHandTransformRight;

    [HideInInspector]
    public Transform playerRigHeadTransform;
    [HideInInspector]
    public Transform playerRigHandLeftTransform;
    [HideInInspector]
    public Transform playerRigHandRightTransform;

    private bool initialized;

    public void AssignToLocalPlayer(Transform playerHead, Transform playerLeftHand, Transform playerRightHand)
    {
        playerRigHeadTransform = playerHead;
        playerRigHandLeftTransform = playerLeftHand;
        playerRigHandRightTransform = playerRightHand;

        initialized = true;
    }

    void Update()
    {
        if (initialized == true && photonView.IsMine)
        {
            //TODO: Sync location of PlayerRig?
            SyncTransform(avatarHeadTransform, playerRigHeadTransform);
            SyncTransform(avatarHandTransformLeft, playerRigHandLeftTransform);
            SyncTransform(avatarHandTransformRight, playerRigHandRightTransform);
        }
    }

    private void SyncTransform(Transform follower, Transform target)
    {
        follower.position = target.position;
        follower.rotation = target.rotation;
    }
}
