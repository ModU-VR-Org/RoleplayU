using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    public GameObject createRoomNameInput;
    public GameObject joinRoomNameInput;

    private NetworkLauncher_NET networkLauncher;

    void Awake()
    {
        networkLauncher = GetComponent<NetworkLauncher_NET>();
    }

    public void JoinRandom()
    {
        networkLauncher.JoinRandomRoom();
    }

    public void CreateRoom()
    {
        string roomName = createRoomNameInput.GetComponent<TMP_InputField>().text;
        networkLauncher.CreateRoom(roomName);
    }

    public void JoinRoom()
    {
        string roomName = joinRoomNameInput.GetComponent<TMP_InputField>().text;
        networkLauncher.JoinRoom(roomName);
    }
}
