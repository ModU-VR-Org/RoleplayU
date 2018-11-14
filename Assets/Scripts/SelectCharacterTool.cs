using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectCharacterTool : MonoBehaviour
{
    private GameObject localAvatar;

    public void SelectCharacter(int characterIndex)
    {
        string username = (string)PhotonNetwork.LocalPlayer.CustomProperties["username"];
        localAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        localAvatar.GetComponent<SelectCharacterTool_NET>().CmdSelectCharacter(username, characterIndex);

        //Change character index in Database 
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("selectedCharacterPost", characterIndex);
        WWW register = new WWW(DatabaseConstants.selectCharacter, form);

        //Change character index in Photon
        Hashtable hash = new Hashtable();
        hash.Add("selectedCharacterIndex", characterIndex);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}
