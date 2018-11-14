using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSaveSlotButton : MonoBehaviour
{
    public SceneMenuPage sceneMenuPage;
    public string saveSlotName;

    public void SetSelectedSave()
    {
        sceneMenuPage.saveString = saveSlotName;
    }
}
