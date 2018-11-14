using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class EnvironmentTool_NET : MonoBehaviour , IInitializable
{
    private EnvironmentMenuPage menuPage;
    private GameObject localPlayerAvatar;

    public void Initialize()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            menuPage = GetComponent<AvatarManager>().playerManager.menu.GetComponent<MenuManager>().environmentMenuPage;
        }
    }

    [PunRPC]
    public void RpcChangeEnvironment(int index)
    {
        localPlayerAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;

        localPlayerAvatar.GetComponent<EnvironmentTool_NET>().menuPage.ChangeEnvironment(index);
    }

}
