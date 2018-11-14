using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Utilities_NET 
{
    public static GameObject GetPlayerAvatar(int actorId)
    {
        return (GameObject)PhotonNetwork.LocalPlayer.Get(actorId).TagObject;
    }

    public static GameObject GetPlayerAvatar(string username)
    {
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            if (username == (string)playerList[i].CustomProperties["username"])
            {
                return (GameObject)playerList[i].TagObject;
            }
        }
        return null;
    }

    public static Player GetPlayer(int actorId)
    {
        return PhotonNetwork.LocalPlayer.Get(actorId);
    }

    public static Player GetPlayer(string username)
    {
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            if (username == (string)playerList[i].CustomProperties["username"])
            {
                return playerList[i];
            }
        }

        return null;
    }

    public static bool LocalPlayerIsDM
    {
        get
        {
            return (string)PhotonNetwork.LocalPlayer.CustomProperties["username"] == (string)PhotonNetwork.CurrentRoom.CustomProperties["host"];
        }
    }
}
