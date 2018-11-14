using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportMenuButtons : MonoBehaviour
{
    public Toggle freeMovementButton;
    public Toggle singleMovementButton;
    public TextMeshProUGUI playerName;

    [HideInInspector]
    public int actorId;

    private TeleportPermissionTool teleportPermissionTool;

    private bool freeMovementIsEnabled;
    private bool singleMovementIsEnabled;

    private void Start()
    {
        teleportPermissionTool = GetComponentInParent<TeleportPermissionTool>();
        freeMovementIsEnabled = true;
    }

    public void InitializeTeleportMenuButtons(int netId, string name)
    {
        actorId = netId;
        playerName.text = name;
    }

    public void FreeTeleportButton()
    {
        freeMovementIsEnabled = !freeMovementIsEnabled;
        teleportPermissionTool.ToggleFreeTeleportRpc(actorId, freeMovementIsEnabled);

        // toggle off single movement, if free teleport was just turned on
        if (freeMovementIsEnabled)
        {
            singleMovementButton.isOn = false;
        }
    }

    public void SingleTeleportButton()
    {
        singleMovementIsEnabled = ! singleMovementIsEnabled;
        teleportPermissionTool.ToggleSingleTeleportRpc(actorId, singleMovementIsEnabled);

        // toggle off free movement, if single movement was just turned on
        if (singleMovementIsEnabled)
        {
            freeMovementButton.isOn = false;
        }
    }

    public void DeactivateSingleTeleportToggle()
    {
        singleMovementIsEnabled = false;
        singleMovementButton.isOn = false;
    }
}
