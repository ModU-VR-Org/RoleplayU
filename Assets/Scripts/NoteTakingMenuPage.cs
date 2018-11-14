using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTakingMenuPage : MenuPageBase
{
    public NoteTakingTool noteTool;

    public override void Start()
    {
        base.Start();
        noteTool = GetComponent<NoteTakingTool>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(true);
    }

    public override void ScrollLeft()
    {
        base.ScrollLeft();
        noteTool.PageDown();
    }
    public override void ScrollRight()
    {
        base.ScrollRight();
        noteTool.PageUp();
    }
}
