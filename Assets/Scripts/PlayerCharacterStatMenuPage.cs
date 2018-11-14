using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCharacterStatMenuPage : MenuPageBase
{
    public GameObject healthPlus_Button;
    public GameObject healthMinus_Button;
    public int currentSelectedHealth = 1;


    public override void OnEnable()
    {
        menuManager.SetScrollButtonsActive(false);
        base.OnEnable();
        ShowCharacterStats();
    }

    public override void BackButton()
    {
        if (Utilities_NET.LocalPlayerIsDM)
        {
            healthPlus_Button.SetActive(false);
            healthMinus_Button.SetActive(false);
        }
        base.BackButton();
    }
    public override void ActivateChild(GameObject child)
    {
        if (Utilities_NET.LocalPlayerIsDM)
        {
            healthPlus_Button.SetActive(false);
            healthMinus_Button.SetActive(false);
        }
        base.ActivateChild(child);
    }

    public void ShowCharacterStats()
    {
        menuManager.statText.SetActive(true);
        menuManager.statTextPanel.SetActive(true);
        menuManager.statTextPanel.transform.SetParent(gameObject.transform);

        if (menuManager.storedCharacterIndex < 0)
        {
            menuManager.statText.GetComponent<TextMeshProUGUI>().text = "Not a character";
        }
        else
        {
            Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();
            currentSelectedHealth = (int)Utilities_NET.GetPlayer(menuManager.storedCharacterUsername).CustomProperties["health"];
            SetCharacterStatText(thisCharacter.characterData, currentSelectedHealth, menuManager.storedCharacterUsername, "");

            if (Utilities_NET.LocalPlayerIsDM)
            {
                healthPlus_Button.SetActive(true);
                healthMinus_Button.SetActive(true);
                healthMinus_Button.transform.SetParent(gameObject.transform);
                healthPlus_Button.transform.SetParent(gameObject.transform);
            }
        }
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

