using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSpawner : MonoBehaviour 
{
    public GameObject avatarPrefab;

    public Transform headTransform;
    public Transform handTransformLeft;
    public Transform handTransformRight;

    private GameObject localAvatar;
    private AvatarManager localAvatarController;

    private PlayerManager playerManager;

    public Transform[] startTransforms;

    void Start () 
	{
        playerManager = GetComponent<PlayerManager>();
       
        int random = Random.Range(0, startTransforms.Length);
        playerManager.playerRig.transform.position = startTransforms[random].position;
        playerManager.playerRig.transform.rotation = startTransforms[random].rotation; 

        localAvatar = PhotonNetwork.Instantiate(avatarPrefab.name, Vector3.zero, Quaternion.identity, 0);

        localAvatar.GetComponent<AvatarDriver>().AssignToLocalPlayer(headTransform, handTransformLeft, handTransformRight);
        localAvatar.GetComponent<AvatarManager>().playerManager = playerManager;  
        
        playerManager.localAvatar = localAvatar;
    }
}
