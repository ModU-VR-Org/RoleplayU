using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnMenuPage : MenuPageBase, IInitializable
{
    public SpawnTool spawnTool;

    public SpawnableModels spawnableModels; //scriptable object. holds all spawnable models

    private int spawnModelPrefabIndex = 0;
    public GameObject spawnButtonPrefab;
    private List<GameObject> spawnedButtonsPool = new List<GameObject>();

    public void Initialize()
    {      
        spawnTool = GetComponent<SpawnTool>();
        LoadCharacterDataFromJson();
        CreateSpawnButtonPool();
        SetSpawnButtonValues();
        gameObject.SetActive(false);
    }

    private void LoadCharacterDataFromJson()
    {
        foreach (GameObject npcModel in spawnableModels.prefabList)
        {
            npcModel.GetComponent<Character>().LoadCharacterData();
        }
    }

    private void CreateSpawnButtonPool()
    {
        for (int i = 0; i < menuManager.menuButtonTransforms.Count; i++)
        {
            //create spawn button, fill its script reference
            GameObject button = Instantiate(spawnButtonPrefab, menuManager.menuButtonTransforms[i].position, menuManager.exampleButtonRotation.rotation);
            button.GetComponent<SpawnButton>().spawnTool = spawnTool;
            button.transform.SetParent(gameObject.transform);
            spawnedButtonsPool.Add(button);
        }
    }

    private void SetSpawnButtonValues()
    {
        for (int i = 0; i < spawnedButtonsPool.Count; i++)
        {
            if (spawnModelPrefabIndex >= spawnableModels.prefabList.Count)
            {
                spawnModelPrefabIndex = 0;
            }
            spawnedButtonsPool[i].GetComponent<SpawnButton>().modelIndex = spawnModelPrefabIndex;
            spawnedButtonsPool[i].GetComponentInChildren<TextMeshProUGUI>().text = spawnableModels.prefabList[spawnModelPrefabIndex].GetComponent<Model>().objectName;
            spawnModelPrefabIndex++;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(true); 
    }

    public override void ScrollLeft()
    {
        ScrollLeftSpawnTool();
        base.ScrollLeft();
    }

    public override void ScrollRight()
    {
        ScrollRightSpawnTool();
        base.ScrollRight();
    }

    public void ScrollLeftSpawnTool()
    {
        spawnModelPrefabIndex -= spawnedButtonsPool.Count * 2;
        if (spawnModelPrefabIndex < 0)
        {
            spawnModelPrefabIndex += spawnableModels.prefabList.Count;
        }

        for (int i = 0; i < spawnedButtonsPool.Count; i++)
        {
            if (spawnModelPrefabIndex >= spawnableModels.prefabList.Count)
            {
                spawnModelPrefabIndex = 0;
            }                    
            spawnedButtonsPool[i].GetComponent<SpawnButton>().modelIndex = spawnModelPrefabIndex;
            spawnedButtonsPool[i].GetComponentInChildren<TextMeshProUGUI>().text = spawnableModels.prefabList[spawnModelPrefabIndex].GetComponent<Model>().objectName;
            spawnModelPrefabIndex++;
        }
    }

    public void ScrollRightSpawnTool()
    {
        if (spawnModelPrefabIndex < 0)
        {
            spawnModelPrefabIndex += spawnableModels.prefabList.Count;
        }
        for (int i = 0; i < spawnedButtonsPool.Count; i++)
        {
            if (spawnModelPrefabIndex >= spawnableModels.prefabList.Count)
            {
                spawnModelPrefabIndex = 0;
            }          
            spawnedButtonsPool[i].GetComponent<SpawnButton>().modelIndex = spawnModelPrefabIndex;
            spawnedButtonsPool[i].GetComponentInChildren<TextMeshProUGUI>().text = spawnableModels.prefabList[spawnModelPrefabIndex].GetComponent<Model>().objectName;
            spawnModelPrefabIndex++;
        }
    }
}
