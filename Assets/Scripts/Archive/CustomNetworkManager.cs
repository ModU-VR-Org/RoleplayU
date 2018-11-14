using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public List<int> connectionIds = new List<int>();
    //public SyncListString playerNames = new SyncListString();


    public static CustomNetworkManager instance;
    // Use this for initialization
    void Start()
    { //may need to use Start
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // called on the server when a new client connects, including the host who connects immediately with connectionId 0
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        connectionIds.Add(conn.connectionId);
    }

    // called on the server when a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //conn.playerControllers[0].gameObject.GetComponent<PlayerController>().RemovePlayer(); //untested!!!!!!! _TESTREMOVED
        base.OnServerDisconnect(conn);
        connectionIds.Remove(conn.connectionId);
    }

    // attempt to get rid of error message when starting a match:
    //public override void OnClientSceneChanged(NetworkConnection conn)
    //{
    //    // do nothing, just overriding the base method to eliminate error
    //}


    // called on a client when they connect
    public override void OnClientConnect(NetworkConnection networkConnection)
    {
        base.OnClientConnect(networkConnection);

        
    }

    // called on a server when it is started
    public override void OnStartServer()
    {
        base.OnStartServer();

        
    }

    
}
