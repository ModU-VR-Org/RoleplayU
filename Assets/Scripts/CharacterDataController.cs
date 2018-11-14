using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class CharacterDataController 
{
    private static string gameDataFileNameRoot = "Data.json";

    public static CharacterData LoadCharacterData(string characterPrefabName, CharacterData currentCharacterData)
    {
        string fileName = characterPrefabName + gameDataFileNameRoot;
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists (filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            CharacterData loadedCharacterData = JsonUtility.FromJson<CharacterData>(dataAsJson);
            return loadedCharacterData;
        }
        else
        {
            Debug.LogError("Cannot load character data for character prefab: " + characterPrefabName);

            return currentCharacterData;
        }
    }
}
