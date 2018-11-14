using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTurning : MonoBehaviour 
{
    public float snapThreshhold = 0.7f;
    public float snapAngle = 15;

    public enum SnapTurnController
    {
        rightController,
        leftController
    }

    public SnapTurnController snapTurnController;

    private Transform playerRigTransform;
    private Transform playerHeadTransform;
    public InputManager inputManager;

    private bool isSnapped;

    void Start () 
	{
        inputManager = GetComponent<InputManager>();
        playerRigTransform = transform;
        playerHeadTransform = GetComponentInChildren<Camera>().gameObject.transform;
	}
	
	void Update () 
	{
        if (snapTurnController == SnapTurnController.rightController)
        {
            float xAxisControllerR = inputManager.controllerRight.XAxis;
            CheckForSnapTurnInput(xAxisControllerR);
        }
        if (snapTurnController == SnapTurnController.leftController)
        {
            float xAxisControllerL = inputManager.controllerLeft.XAxis;
            CheckForSnapTurnInput(xAxisControllerL);
        }
    }

    private void CheckForSnapTurnInput(float xAxisValue)
    {
        if (xAxisValue > snapThreshhold && isSnapped == false)
        {
            playerRigTransform.RotateAround(playerHeadTransform.position, Vector3.up, snapAngle);
            isSnapped = true;
        }
        else if (xAxisValue < -snapThreshhold && isSnapped == false)
        {
            playerRigTransform.RotateAround(playerHeadTransform.position, Vector3.up, -snapAngle);
            isSnapped = true;
        }
        else if (xAxisValue < snapThreshhold && xAxisValue > -snapThreshhold && isSnapped == true)
        {
            isSnapped = false;
        }
    }
}
