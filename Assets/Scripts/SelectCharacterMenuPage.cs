using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacterMenuPage : MenuPageBase, IInitializable
{
    public int characterPrefabIndex;
    private SelectCharacterTool selectCharacterTool;

    public void Initialize()
    {
        selectCharacterTool = GetComponent<SelectCharacterTool>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(false);
        SelectCharacterToolActivate();
    }

    public override void BackButton()
    {
        base.BackButton();
        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            menuManager.imageButtonPool[i].SetActive(false);
        }
    }
    public void SelectCharacterToolActivate()
    {
        for (int i = 0; i < menuManager.imageButtonPool.Count; i++)
        {
            if (characterPrefabIndex >= menuManager.avatarArt.characterPrefabsList.Count)
            {
                characterPrefabIndex = 0;
            }

            ImageButton characterSelectButton = menuManager.imageButtonPool[i].GetComponent<ImageButton>();
            characterSelectButton.lowerText.text = GetCharacterName(characterPrefabIndex); //by default is same as class
            characterSelectButton.upperText.text = "";
            characterSelectButton.image.sprite = GetCharacterSprite(characterPrefabIndex);
            menuManager.imageButtonPool[i].SetActive(true);
            characterPrefabIndex++;

            menuManager.imageButtonPool[i].transform.SetParent(gameObject.transform);

            int index = i;
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.RemoveAllListeners();
            menuManager.imageButtonPool[i].GetComponent<Button>().onClick.AddListener(delegate { SelectCharacter(index); });
        }
    }

    public void SelectCharacter(int buttonIndex) //called when buttons on this menu page are clicked
    {
        selectCharacterTool.SelectCharacter(buttonIndex);
    }

    string GetCharacterName(int characterIndex)
    {
        return menuManager.avatarArt.characterPrefabsList[characterIndex].GetComponent<Character>().characterData.characterName;
    }
    Sprite GetCharacterSprite(int characterIndex)
    {
        return menuManager.avatarArt.characterPrefabsList[characterIndex].GetComponent<Character>().characterPicture;
    }
}
