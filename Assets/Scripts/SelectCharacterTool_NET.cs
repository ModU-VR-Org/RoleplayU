using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class SelectCharacterTool_NET : MonoBehaviourPun, IInitializable {

    private AvatarManager avatarManager;
    private AvatarDriver avatarDriver;
    private int selectedCharacterIndex = -1;

    public string username;
    private string tempCharacterIndex;

    //Player Character
    public GameObject selectedCharacterPrefab;
    public GameObject selectedCharacterHead;
    public GameObject selectedCharacterHandL;
    public GameObject selectedCharacterHandR;

    public GameObject startHead; //fill with start avatar head
    public GameObject[] startHands; //fill with start avatar hands

    private GameObject targetAvatar;

    public void Initialize()
    {
        avatarManager = GetComponent<AvatarManager>();
        avatarDriver = GetComponent<AvatarDriver>();
        InitializePlayer();
    }

    void InitializePlayer()
    {
        if (photonView.IsMine)
        {
            StartCoroutine("InitializeLocalPlayerData");
        }
        else //only runs on Local Non-Authoritive Players
        {
            if (!GetComponent<PhotonView>().Owner.CustomProperties.ContainsKey("selectedCharacterIndex"))
            {
                return; //might want to add multiple trys at this, to give time for network lag
            }
            selectedCharacterIndex = (int)GetComponent<PhotonView>().Owner.CustomProperties["selectedCharacterIndex"];

            SetAvatarCharacterArt(selectedCharacterIndex);
        }
    }

    IEnumerator InitializeLocalPlayerData()
    {
        // this is set by login menu
        if (PlayerPrefs.HasKey("loginUsername"))
        {
            username = PlayerPrefs.GetString("loginUsername", "");
        }

        //Get LocalPlayer's stored CharacterIndex from database
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        WWW data = new WWW(DatabaseConstants.initializePlayer, form);
        yield return data;

        tempCharacterIndex = data.text;

        //Set Avatar CharacterIndex from database value
        if (tempCharacterIndex == null || tempCharacterIndex == "")
        {
            selectedCharacterIndex = -1; //No Character Selected
        }
        else
        {
            selectedCharacterIndex = Convert.ToInt32(tempCharacterIndex, 10);

            photonView.RPC("SetAvatarCharacterArt", RpcTarget.All, selectedCharacterIndex);

        }
        avatarManager.playerManager.InitializePlayerInfo(username, selectedCharacterIndex);
    }

    [PunRPC]
    public void SetAvatarCharacterArt(int selectedCharacter)
    {
        if (selectedCharacter == -1)
        {
            return;
        }
        else
        {
            DeactivatePreviousCharacterArt();

            selectedCharacterIndex = selectedCharacter;
            selectedCharacterPrefab = avatarManager.avatarArt.characterPrefabsList[selectedCharacterIndex];

            //Activate Head Art
            GameObject obj = Instantiate(selectedCharacterPrefab, avatarDriver.avatarHeadTransform.position, avatarDriver.avatarHeadTransform.rotation);
            obj.transform.SetParent(avatarDriver.avatarHeadTransform);
            selectedCharacterHead = obj;

            //Deactivate head for LocalPlayer's Avatar so it doesn't block their vision
            if (photonView.IsMine)
            {
                selectedCharacterHead.SetActive(false);
            }

            //Activate Hand Art
            GameObject handL = Instantiate(avatarManager.avatarArt.characterPrefabHandsList[selectedCharacterIndex].handLeft,
               avatarDriver.avatarHandTransformLeft.position, avatarDriver.avatarHandTransformLeft.rotation);
            handL.transform.SetParent(avatarDriver.avatarHandTransformLeft);
            selectedCharacterHandL = handL;

            GameObject handR = Instantiate(avatarManager.avatarArt.characterPrefabHandsList[selectedCharacterIndex].handRight,
               avatarDriver.avatarHandTransformRight.position, avatarDriver.avatarHandTransformRight.rotation);
            handR.transform.SetParent(avatarDriver.avatarHandTransformRight);
            selectedCharacterHandR = handR;
        }
    }

    private void DeactivatePreviousCharacterArt()
    {
        if (selectedCharacterHead != null)
        {
            Destroy(selectedCharacterHead);
            Destroy(selectedCharacterHandL);
            Destroy(selectedCharacterHandR);
        }
        else
        {
            //Deactivate starting character art
            startHead.SetActive(false);
            startHands[0].GetComponent<MeshRenderer>().enabled = false;
            startHands[1].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    //Switch Character
    public void CmdSelectCharacter(string username, int characterIndex)
    {
        photonView.RPC("RpcSelectCharacter", RpcTarget.All, username, characterIndex);
    }

    [PunRPC]
    public void RpcSelectCharacter(string username, int characterIndex)  //NOTE: On player joins, this is synced via Initialize functions
    {
        targetAvatar = Utilities_NET.GetPlayerAvatar(username);
        targetAvatar.GetComponent<SelectCharacterTool_NET>().SetAvatarCharacterArt(characterIndex);
    }
}
