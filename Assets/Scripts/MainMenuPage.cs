using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPage : MenuPageBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(false);
    }
}
