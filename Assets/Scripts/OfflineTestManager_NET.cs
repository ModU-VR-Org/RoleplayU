using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OfflineTestManager_NET : MonoBehaviour
{
    private void Awake()
    {
        if (PhotonNetwork.InRoom)
        {
            return;
        }

        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 1 });
    }
}
