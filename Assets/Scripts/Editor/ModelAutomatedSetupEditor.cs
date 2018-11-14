using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ModelAutomatedSetup))]
[CanEditMultipleObjects]
public class ModelAutomatedSetupEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        ModelAutomatedSetup modelSetupScript = (ModelAutomatedSetup)target;

        if (GUILayout.Button("Remove Automated Components"))
        {
            modelSetupScript.RemoveComponents();
        }

        if (GUILayout.Button("Add Model Components"))
        {
            modelSetupScript.AddMissingModelScripts();
        }

        if (GUILayout.Button("Populate Model Components"))
        {
            modelSetupScript.PopulateScripts();
        }
    }
}
