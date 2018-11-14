using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarManager : MonoBehaviourPun
{
    public AvatarArtAndCharacterData avatarArt;

    public PlayerManager playerManager;  

    public GameObject teleportIndicator;

    private IInitializable[] networkedTools;

    void Start()
    {
        photonView.Owner.TagObject = gameObject; //makes Avatar gameobjects referenceable by player

        InitializeNetworkedTools();
    }

    private void InitializeNetworkedTools()
    {
        networkedTools = GetComponents<IInitializable>();
        foreach (IInitializable networkTool in networkedTools)
        {
            networkTool.Initialize();
        }
    }
  
}
