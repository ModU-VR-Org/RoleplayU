using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcMenuPage : MenuPageBase, IInitializable
{
    public GameObject npcHealthPositive_Button;
    public GameObject npcHealthNegative_Button;

    private GameObject selectedObject;
    public TextMeshProUGUI npcStatText;

    public void Initialize()
    {
        menuManager.selectionTool.OnSelection += ChangeSelectedModel;
    }

    private void ChangeSelectedModel(GameObject obj)
    {
            selectedObject = obj;

            if (selectedObject != null)
            {
                NPCStatMenu(); //could check if it's a character here to save minor performance
            }   
    }


    public override void OnEnable()
    {
        base.OnEnable();
        NPCStatMenu();
    }


    public override void BackButton()
    {
        BackButton_NPCStatTool();
        base.BackButton();
    }

    public void NPCStatMenu()
    {
        if (selectedObject != null)
        {
            Character thisCharacter = selectedObject.GetComponent<Character>();
            CharacterData characterData = thisCharacter.characterData;
            if (thisCharacter != null && characterData != null)
            {
                npcHealthNegative_Button.SetActive(true);
                npcHealthPositive_Button.SetActive(true);

                SetCharacterStatText(characterData, characterData.currentHealth, characterData.characterName, characterData.description);
            }
            else
            {
                npcStatText.text = "No NPC Character Selected";
            }
        }
        else
        {
            npcStatText.text = "No NPC Character Selected";
        }
    }

    private void SetCharacterStatText(CharacterData characterData, int currentHealth, string name, string description = "")
    {
        npcStatText.text =
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

    public void BackButton_NPCStatTool()
    {
        npcHealthNegative_Button.SetActive(false);
        npcHealthPositive_Button.SetActive(false);
    }

    public void ChangeNPCHealth(bool positive)
    {
        if(selectedObject != null)
        {
            CharacterData characterData = selectedObject.GetComponent<Character>().characterData;
            if (characterData != null)
            {
                if (positive)
                {
                    characterData.currentHealth++;
                }
                else
                {
                    characterData.currentHealth--;
                }

                SetCharacterStatText(characterData, characterData.currentHealth, characterData.characterName, characterData.description);
            }
        }
        else
        {
            npcStatText.text = "No NPC Character Selected";
        }
    }
}
