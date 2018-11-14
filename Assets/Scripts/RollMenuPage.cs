using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMenuPage : MenuPageBase, IInitializable
{
    public RollTool rollTool;

    public void Initialize()
    {
        rollTool.InitializeRollButtonColors();
    }
}
