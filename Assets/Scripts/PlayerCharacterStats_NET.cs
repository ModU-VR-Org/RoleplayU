using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerCharacterStats_NET : MonoBehaviour, IInitializable
{
    private MenuManager menuManager;
    private GameObject localPlayerAvatar;
    private int currentSelectedHealth;

    public void Initialize()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            menuManager = GetComponent<AvatarManager>().playerManager.menu.GetComponent<MenuManager>();
        }
    }

    [PunRPC]
    public void RpcRefreshMenuPlayerHealth()
    {
        localPlayerAvatar = (GameObject)PhotonNetwork.LocalPlayer.TagObject;

        localPlayerAvatar.GetComponent<PlayerCharacterStats_NET>().UpdateCharacterStatTool();
    }

    public void UpdateCharacterStatTool()
    {
        Player[] playerList = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerList.Length; i++)
        {
            if (menuManager.storedCharacterUsername == (string)playerList[i].CustomProperties["username"])
            {
                currentSelectedHealth = (int)playerList[i].CustomProperties["health"];
            }
        }

        Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();
        SetCharacterStatText(thisCharacter.characterData, currentSelectedHealth, menuManager.storedCharacterUsername, "");
    }

    public void SetCharacterStatText(CharacterData characterData, int currentHealth, string name, string description = "")
    {
        menuManager.statText.GetComponent<TextMeshProUGUI>().text =
        name + " the " + characterData.characterRace + " " + characterData.characterClass +
        "\n" + "AC = " + characterData.stat_armorClass + "      Health: " + currentHealth + "/" + characterData.stat_healthMax + " Speed:" + characterData.stat_speed +
        "\n \n" + "Strength: " + characterData.stat_strength + "(" + characterData.stat_strengthMod + ") |" +
        " " + "Dexterity: " + characterData.stat_dexterity + "(" + characterData.stat_dexterityMod + ")" +
        "\n" + "Constitution: " + characterData.stat_constitution + "(" + characterData.stat_constitutionMod + ") |" +
        " " + "Intelligence: " + characterData.stat_intelligence + "(" + characterData.stat_intelligenceMod + ")" +
        "\n" + "Wisdom: " + characterData.stat_wisdom + "(" + characterData.stat_wisdomMod + ") |" +
        " " + "Charisma: " + characterData.stat_charisma + "(" + characterData.stat_charismaMod + ")" +
        "\n" + description;
    }
}
