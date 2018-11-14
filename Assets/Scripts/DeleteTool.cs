using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeleteTool : MonoBehaviour, IInitializable
{
    public MenuManager menuManager;
    public GameObject selectedModel;

    public SpawnTool spawnTool;
    public SelectionTool selectionTool;

    public void Initialize()
    {
        selectionTool = menuManager.selectionTool;
        selectionTool.OnSelection += SetSelectedModel;
    }

    private void SetSelectedModel(GameObject obj)
    {
        selectedModel = obj;
    }

    public void DeleteModel()
    {
        if (selectedModel != null)
        {
            spawnTool.modelsSpawned.Remove(selectedModel);
            PhotonNetwork.Destroy(selectedModel);
            selectedModel = null;
            selectionTool.SetSelectionToNull();
        }
    }
}
