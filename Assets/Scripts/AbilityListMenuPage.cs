using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityListMenuPage : MenuPageBase
{
    public GameObject childPage;

    private int abilitiesButtonIndex;

    public override void OnEnable()
    {
        base.OnEnable();
        AbilitiesListToolActivate();
    }

    public override void BackButton()
    {
        BackButton_CharacterAbilityListTool();
        base.BackButton();
    }

    public override void ScrollLeft()
    {
        ScrollLeftAbilitiesTool();
        base.ScrollLeft();
    }

    public override void ScrollRight()
    {
        ScrollRightAbilitiesTool();
        base.ScrollRight();
    }

    public override void CloseButton()
    {
        base.CloseButton();
        BackButton_CharacterAbilityListTool();
    }

    public void AbilitiesListToolActivate()
    {
        Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();

        if (thisCharacter.characterData.abilitiesList.Count == 0)
        {
            //Debug.Log("Character has no abilities");
            return;
        }

        //activate buttons and give them ability stats
        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            menuManager.imageButtonPool[i].SetActive(true);
            menuManager.imageButtonPool[i].transform.SetParent(transform);
        }

        SetAbilityListButtons(thisCharacter);
      
        menuManager.SetScrollButtonsActive(true);
    }

    private void SetAbilityListButtons(Character thisCharacter)
    {
        ImageButton characterSelectButton;

        if (abilitiesButtonIndex < 0)
        {
            abilitiesButtonIndex += menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>().characterData.abilitiesList.Count;
        }

        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            if (abilitiesButtonIndex >= thisCharacter.characterData.abilitiesList.Count)
            {
                abilitiesButtonIndex = 0;
            }
            //create button matching Model info on spawnList gameobject 
            characterSelectButton = menuManager.imageButtonPool[i].GetComponent<ImageButton>();

            //characterSelectButton.abilityName = thisCharacter.characterData.abilitiesList[abilitiesButtonIndex].name;
            //characterSelectButton.abilityHighlightedInfo = thisCharacter.characterData.abilitiesList[abilitiesButtonIndex].highlightDescription;
            //characterSelectButton.abilityInfo = thisCharacter.characterData.abilitiesList[abilitiesButtonIndex].description;
            menuManager.imageButtonPool[i].GetComponentInChildren<TextMeshProUGUI>().text = thisCharacter.characterData.abilitiesList[abilitiesButtonIndex].name;
            characterSelectButton.lowerText.text = "";
            characterSelectButton.image.sprite = null;

            int abilityIndex = abilitiesButtonIndex;
            menuManager.imageButtonPool[i].transform.SetParent(gameObject.transform);
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.RemoveAllListeners();
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.AddListener(delegate { ShowAbilityStats(abilityIndex); });

            abilitiesButtonIndex++;
        }
    }

    public void ShowAbilityStats(int abilityIndex)
    {
        menuManager.storedAbilityIndex = abilityIndex;
        abilitiesButtonIndex -= menuManager.imageButtonPool.Count;

        gameObject.SetActive(false);
        childPage.SetActive(true);
    }

    public void ScrollLeftAbilitiesTool()
    {
        Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();
        abilitiesButtonIndex -= menuManager.imageButtonPool.Count * 2;

        if (abilitiesButtonIndex < 0)
        {
            abilitiesButtonIndex += thisCharacter.characterData.abilitiesList.Count;
        }

        SetAbilityListButtons(thisCharacter);
    }

    public void ScrollRightAbilitiesTool()
    {
        Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();
        SetAbilityListButtons(thisCharacter);
    }

    public void BackButton_CharacterAbilityListTool()
    {
        abilitiesButtonIndex -= menuManager.imageButtonPool.Count;
        if (abilitiesButtonIndex < 0)
        {
            abilitiesButtonIndex += menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>().characterData.abilitiesList.Count;

        }
    }
}
