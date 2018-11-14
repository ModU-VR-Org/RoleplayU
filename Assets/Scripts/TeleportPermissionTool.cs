using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportPermissionTool : MonoBehaviour
{
    public TeleportMenuPage teleportMenuPage;

    public void ToggleFreeTeleportRpc(int actorId, bool isEnabled)
    {
        Utilities_NET.GetPlayerAvatar(actorId).GetComponent<TeleportStatus_NET>().FireToggleFreeTeleportRPC(actorId, isEnabled);
    }

    public void ToggleSingleTeleportRpc(int actorId, bool isEnabled)
    {
        Utilities_NET.GetPlayerAvatar(actorId).GetComponent<TeleportStatus_NET>().FireToggleSingleTeleportRPC(actorId, isEnabled);
    }

    public void UseSingleTeleport(GameObject playerObject, bool enabledStatus)
    {
        ToggleSingleTeleportRpc(PhotonNetwork.LocalPlayer.ActorNumber, false);
    }
}