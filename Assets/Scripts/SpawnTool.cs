using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnTool : MonoBehaviour, IInitializable
{
    public MenuManager menuManager;
    public SpawnMenuPage spawnMenuPage;

    private InputManager inputManager;
    private GameObject previewObject;
    private int spawnedModelIndex;

    public List<GameObject> modelsSpawned = new List<GameObject>();

    public void Initialize ()
    {
        menuManager = GetComponentInParent<MenuManager>();
        inputManager = menuManager.inputManager;
        inputManager.controllerRight.OnTriggerUnClicked += TriggerSpawnUnClick;
    }

    private void OnEnable()
    {
        inputManager.controllerRight.OnTriggerUnClicked += TriggerSpawnUnClick;
    }

    private void OnDisable()
    {
        inputManager.controllerRight.OnTriggerUnClicked -= TriggerSpawnUnClick;
    }

    public void SpawnLocalPreview(int modelIndex, Vector3 pos) 
    {
        Debug.Log("spawn local");
        menuManager.selectionTool.DeselectModel();

        previewObject = Instantiate(spawnMenuPage.spawnableModels.prefabList[modelIndex], pos, menuManager.inputManager.controllerRight.controller.transform.rotation);
        previewObject.GetComponent<Collider>().enabled = false;
        previewObject.transform.SetParent(menuManager.inputManager.controllerRight.controller.transform); //when object spawned, it moves with hand until trigger unheld

        spawnedModelIndex = modelIndex;
    }

    private void TriggerSpawnUnClick()
    {
        if (previewObject != null)
        {
            Debug.Log(previewObject.name);
            previewObject.transform.SetParent(null);

            SpawnNetworkedModel(spawnedModelIndex, previewObject.transform.position, previewObject.transform.rotation, previewObject.transform.localScale);

            Destroy(previewObject);
            previewObject = null;
        }
    }

    public void SpawnNetworkedModel(int model, Vector3 pos, Quaternion rot, Vector3 scl)
    {
        Debug.Log("spawn network");
        string nameOfSpawnObject = spawnMenuPage.spawnableModels.prefabList[model].name; 
        GameObject obj = PhotonNetwork.Instantiate(nameOfSpawnObject, pos, rot, 0);
        obj.transform.localScale = scl;

        // set the modelId for saving and loading
        obj.GetComponent<Model>().modelId = model;
        modelsSpawned.Add(obj);
    }
}
