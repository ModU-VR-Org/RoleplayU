using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class PlayerSpawn_NetworkTest : MonoBehaviour 
{
    public GameObject playerPrefab;
    public Text sendRateText;
    public Text serializeRateText;
    void Start () 
	{
        for(int i = 0; i < 20; i++)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation);
        }
	}

    private void Update()
    {
        float byteRate = PhotonNetwork.NetworkingClient.LoadBalancingPeer.TrafficStatsIncoming.TotalPacketBytes / Time.time;
        sendRateText.text = byteRate.ToString();
        //serializeRateText.text = PhotonNetwork.SerializationRate.ToString();
    }


}
