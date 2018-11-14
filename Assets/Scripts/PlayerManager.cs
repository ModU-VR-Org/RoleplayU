using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerRig;
    public GameObject menu;
    public GameObject localAvatar;

    public void InitializePlayerInfo(string username, int selectedCharacterIndex)
    {
        int health;
        if (selectedCharacterIndex == -1)
        {
            health = 10; //arbitrary number
        }
        else
        {
            health = menu.GetComponent<MenuManager>().avatarArt.characterPrefabsList[selectedCharacterIndex].GetComponent<Character>().characterData.currentHealth;
        }

        Hashtable hash = new Hashtable();
        hash.Add("username", username);
        hash.Add("selectedCharacterIndex", selectedCharacterIndex);
        hash.Add("health", health);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        //Assign first player to join a room as the DM (Note: This seems clunky, think of how to make more robust)
        if (PhotonNetwork.LocalPlayer.IsMasterClient) //only fires when you're the first person to join/create an empty room
        {
            AssignDM(username);
        }

        //if DM rejoins, give them DM permisions
        StartCoroutine(InitializeDM(1f));
    }

    private static void AssignDM(string username) 
    {        
            string hostUsername1 = username;
            Hashtable hash1 = new Hashtable();
            hash1.Add("host", hostUsername1);
            hash1.Add("rollPermission", true);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash1, null, null);           
    }

    IEnumerator InitializeDM(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Utilities_NET.LocalPlayerIsDM)
        {
            GameObject localPlayer = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
            menu.GetComponent<MenuManager>().InitializeMenuForHost();
            //Debug.Log("You are the DM");
        }
    }   
}
