using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Sprite characterPicture;

    [SerializeField]
    public CharacterData characterData;

    void Start ()
    {
        CalculateModifiers();
	}

    public void LoadCharacterData()
    {
        characterData = CharacterDataController.LoadCharacterData(gameObject.name, characterData);
    }

    void CalculateModifiers()
    {
        //TODO: auto translate all character stats to their correct modifiers (example below)
        //if (stat_constitution == 9 || stat_constitution == 10)
        //{
        //    stat_constitutionMod = 0;
        //}
    }	
}
