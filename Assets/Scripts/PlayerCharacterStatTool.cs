using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerCharacterStatTool : MonoBehaviour
{
    public PlayerCharacterStatMenuPage menuPage;
    private GameObject localAvatar;

    public void ChangeCharacterHealth(bool positive) //2 buttons childed to this Menu Page invoke this function on click
    {
        if (Utilities_NET.LocalPlayerIsDM)
        {
            if (positive)
            {
                ChangePlayerHealth(menuPage.menuManager.storedCharacterUsername, 1);
            }
            else
            {
                ChangePlayerHealth(menuPage.menuManager.storedCharacterUsername, -1);
            }
        }
    }

    public void ChangePlayerHealth(string selectedPlayerUsername, int increment)
    {
        Player targetPlayer = Utilities_NET.GetPlayer(selectedPlayerUsername);

        Hashtable hash = new Hashtable();
        int health = (int)targetPlayer.CustomProperties["health"] + increment;
        hash.Add("health", health);
        targetPlayer.SetCustomProperties(hash);

        localAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        localAvatar.GetComponent<PhotonView>().RPC("RpcRefreshMenuPlayerHealth", RpcTarget.All);
    }   
}
