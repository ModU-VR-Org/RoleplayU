using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerCharacterListMenuPage : MenuPageBase
{
    public GameObject childPage;


    public override void OnEnable()
    {
        PlayerCharacterListToolActivate();
        base.OnEnable();
    }


    public override void BackButton()
    {
        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            menuManager.imageButtonPool[i].SetActive(false);
        }
        base.BackButton();
    }

    public void PlayerCharacterListToolActivate()
    {
        menuManager.SetScrollButtonsActive(false);

        Player[] playerList = PhotonNetwork.PlayerList;  //NOTE: ACCESSING PLAYERLIST IS VERY INNEFICIENT, ITS REFERENCE SHOULD BE CACHED like the below
        ImageButton imageButtonScript;

        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length) //NOTE: CURRENTLY DOES NOT HANDLE MORE THAN 6 PLAYERS, DOES NOT REFRESH WITHOUT LEAVING THIS MENUPAGE 
            {
                string playerUsername = (string)playerList[i].CustomProperties["username"];
                int playerSelectedCharacter = (int)playerList[i].CustomProperties["selectedCharacterIndex"];
                if ((int)playerList[i].CustomProperties["selectedCharacterIndex"] <= -1)
                {
                    menuManager.imageButtonPool[i].GetComponentInChildren<TextMeshProUGUI>().text = playerUsername + "|No Char";
                }
                else
                {
                    //Make the buttons specific to this MenuPage
                    imageButtonScript = menuManager.imageButtonPool[i].GetComponent<ImageButton>();

                    imageButtonScript.lowerText.text = GetCharacterClass(playerSelectedCharacter);
                    imageButtonScript.upperText.text = playerUsername;
                    imageButtonScript.image.sprite = GetCharacterSprite(playerSelectedCharacter);

                    menuManager.imageButtonPool[i].SetActive(true);

                    int local = i;
                    menuManager.imageButtonPool[i].transform.SetParent(gameObject.transform);
                    menuManager.imageButtonPool[i].GetComponent<Button>().onClick.RemoveAllListeners();
                    menuManager.imageButtonPool[i].GetComponent<Button>().onClick.AddListener(delegate { ShowCharacterStats(local); });
                }
            }
            else
            {
                menuManager.imageButtonPool[i].SetActive(false);
            }
        }
    }

    string GetCharacterClass(int selectedCharacter)
    {
        return menuManager.avatarArt.characterPrefabsList[selectedCharacter].GetComponent<Character>().characterData.characterClass;

    }
    Sprite GetCharacterSprite(int selectedCharacter)
    {
        return menuManager.avatarArt.characterPrefabsList[selectedCharacter].GetComponent<Character>().characterPicture;
    }

    public void ShowCharacterStats(int buttonIndex)
    {
        Player[] playerList = PhotonNetwork.PlayerList;
        menuManager.storedCharacterUsername = (string)playerList[buttonIndex].CustomProperties["username"];
        menuManager.storedCharacterIndex = (int)playerList[buttonIndex].CustomProperties["selectedCharacterIndex"];

        childPage.SetActive(true);
        gameObject.SetActive(false);
    }
}

