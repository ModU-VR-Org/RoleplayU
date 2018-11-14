using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

[CustomEditor(typeof(Character))]
[CanEditMultipleObjects]
public class CharacterEditor : Editor 
{
    private string projectFilePath = "/StreamingAssets/";
    private string fileNameRoot = "Data.json";

    public override void OnInspectorGUI()
    {
        Character characterScript = (Character)target;

        if (GUILayout.Button("Save Character Data To Json"))
        {
            SaveCharacterData(characterScript);
            
        }

        if (GUILayout.Button("Load Character Data From Json"))
        {
            LoadCharacterData(characterScript);
        }

        DrawDefaultInspector();
    }

    private void LoadCharacterData(Character characterScript)
    {
        string fileName = characterScript.gameObject.name + fileNameRoot;

        string filePath = Application.dataPath + projectFilePath + fileName;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            characterScript.characterData = JsonUtility.FromJson<CharacterData>(dataAsJson);
        }
        else
        {
            Debug.LogError("No JSON file by this name exists.");
        }
    }

    private void SaveCharacterData(Character characterScript)
    {
        string fileName = characterScript.gameObject.name + fileNameRoot;

        string filePath = Application.dataPath + projectFilePath + fileName;

        string dataAsJson = JsonUtility.ToJson(characterScript.characterData);

        File.WriteAllText(filePath, dataAsJson);

        AssetDatabase.Refresh();

        Debug.Log("Saved to JSON, fileName: '" + fileName + "'");
    }
}





