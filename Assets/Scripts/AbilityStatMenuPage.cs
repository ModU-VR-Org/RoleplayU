using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityStatMenuPage : MenuPageBase
{
    private string abilityName;
    private string abilityHighlightedInfo;
    private string abilityInfo;

    public GameObject abilitiesStatTextPanel;

    public override void OnEnable()
    {
        base.OnEnable();
        ShowAbilityStats();
    }

    public void ShowAbilityStats()
    {
        Character thisCharacter = menuManager.avatarArt.characterPrefabsList[menuManager.storedCharacterIndex].GetComponent<Character>();
        abilityName = thisCharacter.characterData.abilitiesList[menuManager.storedAbilityIndex].name;
        abilityHighlightedInfo = thisCharacter.characterData.abilitiesList[menuManager.storedAbilityIndex].highlightDescription;
        abilityInfo = thisCharacter.characterData.abilitiesList[menuManager.storedAbilityIndex].description;

        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            menuManager.imageButtonPool[i].SetActive(false);
        }

        abilitiesStatTextPanel.SetActive(true);
        abilitiesStatTextPanel.transform.SetParent(transform);
        abilitiesStatTextPanel.GetComponentInChildren<TextMeshProUGUI>().text = abilityName + "\n" + abilityHighlightedInfo + "\n" + abilityInfo;
        menuManager.SetScrollButtonsActive(false);
    }
}
