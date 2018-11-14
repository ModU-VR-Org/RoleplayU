using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SceneSaveLoadManager : MonoBehaviour
{
    public SpawnTool spawnTool;

    private List<GameObject> modelsSpawned;

    private SceneMenuPage sceneMenuPage;

    void Start()
    {
        sceneMenuPage = GetComponent<SceneMenuPage>();

        modelsSpawned = spawnTool.modelsSpawned;
    }

    private SceneSave CreateSceneSaveObject()
    {
        SceneSave save = new SceneSave();
        foreach (GameObject model in modelsSpawned)
        {
            save.modelIds.Add(model.GetComponent<Model>().modelId);

            save.modelXPositions.Add(model.transform.position.x);
            save.modelYPositions.Add(model.transform.position.y);
            save.modelZPositions.Add(model.transform.position.z);

            save.modelXRotations.Add(model.transform.rotation.eulerAngles.x);
            save.modelYRotations.Add(model.transform.rotation.eulerAngles.y);
            save.modelZRotations.Add(model.transform.rotation.eulerAngles.z);

            save.modelXScales.Add(model.transform.localScale.x);
            save.modelYScales.Add(model.transform.localScale.y);
            save.modelZScales.Add(model.transform.localScale.z);
        }
        return save;
    }

    public void SaveScene(string saveString)
    {
        SceneSave save = CreateSceneSaveObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveString);
        bf.Serialize(file, save);
        file.Close();

        sceneMenuPage.loadConfirmation.SetActive(false);
        sceneMenuPage.saveConfirmation.SetActive(true);
        Invoke("DisableConfirmations", 1);
    } 

    public void LoadScene(string saveString)
    {
        if (File.Exists(Application.persistentDataPath + saveString))
        {
            int length = modelsSpawned.Count;
            for (int i = 0; i < length; i++)
            {
                PhotonNetwork.Destroy(modelsSpawned[0]);
                modelsSpawned.RemoveAt(0);
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveString, FileMode.Open);
            SceneSave save = (SceneSave)bf.Deserialize(file);
            file.Close();

            for (int i = 0; i < save.modelIds.Count; i++)
            {
                int modelId = save.modelIds[i];
                Vector3 pos = new Vector3(save.modelXPositions[i], save.modelYPositions[i], save.modelZPositions[i]);
                Quaternion rot = Quaternion.Euler(save.modelXRotations[i], save.modelYRotations[i], save.modelZRotations[i]);
                Vector3 scl = new Vector3(save.modelXScales[i], save.modelYScales[i], save.modelZScales[i]);

                spawnTool.SpawnNetworkedModel(modelId, pos, rot, scl);
            }

            sceneMenuPage.saveConfirmation.SetActive(false);
            sceneMenuPage.loadConfirmation.SetActive(true);
            Invoke("DisableConfirmations", 1);
        }
        else
        {
            //Debug.Log("Not saved!");
        }
    }

    private void DisableConfirmations()
    {
        sceneMenuPage.saveConfirmation.SetActive(false);
        sceneMenuPage.loadConfirmation.SetActive(false);
    } 
}
