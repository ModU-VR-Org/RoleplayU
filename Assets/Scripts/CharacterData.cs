using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public string characterRace;
    public string characterClass;
    public string description; //only used for NPCs
    public int stat_armorClass = 10;
    public int stat_strength = 10;
    public int stat_dexterity = 10;
    public int stat_constitution = 10;
    public int stat_intelligence = 10;
    public int stat_wisdom = 10;
    public int stat_charisma = 10;

    public int stat_strengthMod = 0;
    public int stat_dexterityMod = 0;
    public int stat_constitutionMod = 0;
    public int stat_intelligenceMod = 0;
    public int stat_wisdomMod = 0;
    public int stat_charismaMod = 0;

    public int stat_healthMax = 8;
    public int currentHealth = 8;
    public int stat_speed = 30;

    [System.Serializable]
    public class Ability
    {
        public string name;
        public string highlightDescription;

        [TextArea (5,10)]
        public string description;
    }
    [SerializeField]
    public List<Ability> abilitiesList;
}
