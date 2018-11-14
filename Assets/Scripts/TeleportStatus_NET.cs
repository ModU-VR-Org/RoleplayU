using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeleportStatus_NET : MonoBehaviourPun, IInitializable
{
    public GameObject teleportIndicator;

    private Teleporter teleporter;

    private AvatarManager avatarManager;

    private GameObject localPlayerAvatar;

    public void Initialize()
    {
        avatarManager = GetComponent<AvatarManager>();

        if (photonView.IsMine)
        {
            teleporter = avatarManager.playerManager.playerRig.GetComponentInChildren<Teleporter>();
            teleporter.OnSingleTeleport += UseSingleTeleport;
        }
    }

    public void FireToggleFreeTeleportRPC(int actorId, bool isEnabled)
    {
        photonView.RPC("ToggleFreeTeleport", Utilities_NET.GetPlayer(actorId), isEnabled);
    }

    [PunRPC]
    public void ToggleFreeTeleport(bool isEnabled)
    {
        teleporter.Pointer.freeTeleport = isEnabled;
    }

    private void UseSingleTeleport()
    {
        photonView.RPC("ToggleSingleTeleport", RpcTarget.All, photonView.Owner.ActorNumber, false);
    }

    public void FireToggleSingleTeleportRPC(int actorId, bool isEnabled)
    {
        photonView.RPC("ToggleSingleTeleport", RpcTarget.All, actorId, isEnabled);
    }

    [PunRPC]
    public void ToggleSingleTeleport(int actorId, bool isEnabled)
    {

        // if the avatar belongs to local player, toggle single teleport on or off
        if (photonView.IsMine)
        {
            teleporter.Pointer.singleTeleport = isEnabled;
        }

        // for all players, toggle the single-teleport visual
        if (isEnabled)
        {
            teleportIndicator.SetActive(true);
        }
        else
        {
            StopCoroutine("ToggleTeleportVisualDelayed");
            StartCoroutine(ToggleTeleportVisualDelayed(2.0f));
            localPlayerAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
            localPlayerAvatar.GetComponent<AvatarManager>().playerManager.menu.GetComponentInChildren<TeleportMenuPage>(true).UpdateTeleportMenu(actorId);
        }
    }

    private IEnumerator ToggleTeleportVisualDelayed(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        teleportIndicator.SetActive(false);
    }
}
