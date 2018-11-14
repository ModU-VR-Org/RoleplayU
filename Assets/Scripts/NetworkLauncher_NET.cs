using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkLauncher_NET : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private byte maxPlayersPerRoom = 6;

    // Client version number, allows separation by gameVersion for breaking changes
    string gameVersion = "1";
    bool isConnecting;

    public GameObject blackOutQuad;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;  // allows using PhotonNetwork.LoadLevel() and syncs level automatically for users in same room

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        if(PhotonNetwork.JoinRoom(roomName))
        {
            //blackOutQuad.SetActive(true);
        }
    }

    public void JoinRandomRoom()
    {
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.JoinRandomRoom())
            {
                //blackOutQuad.SetActive(true);
            }
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("DefaultRoom", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        blackOutQuad.SetActive(true);

        Invoke("LoadOnlineScene", 0.3f);
    }

    private void LoadOnlineScene()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    // unused callbacks:

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Debug.Log("Join Room Failed!");
    }

    public override void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }
}
