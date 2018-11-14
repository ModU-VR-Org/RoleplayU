using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMenuPage : MenuPageBase
{
    public GameObject saveConfirmation;
    public GameObject loadConfirmation;
    public string saveString;

    private SceneSaveLoadManager saveLoadManager;

    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(false);
    }

    public override void Start()
    {
        base.Start();

        saveLoadManager = GetComponent<SceneSaveLoadManager>();
    }

    public void SaveGameButton()
    {
        if (saveString != null)
        {
            saveLoadManager.SaveScene(saveString);
        }
    }
    public void LoadGameButton()
    {
        if (saveString != null)
        {
            saveLoadManager.LoadScene(saveString);
        }
    }
}
