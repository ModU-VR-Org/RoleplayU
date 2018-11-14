using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//REFACT
//This class currently does 2 things, and is located on Controller(left) and Controller(right)
//1. It provides the SteamVR "device" access which allows axis inputs and haptics in other scripts
//2. It implements snap turning control via the stick
//We should seperate these two bits of functionality into 2 classes, and rename both to better reflect this

    //Scripts that depend on this:
    //CanvasInputManager
    //Keyboard
    //Mallet

public class DeviceInput : MonoBehaviour
{
    public Transform playerTransform;
    public Transform playerHeadTransform;
    ushort duration = 500;

    public bool snapTurning;
    // snap point of joystick for snap turning... set between 0 and 1, 0.7 seems like good spot:
    private float snapPoint = 0.7f;

    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;

    private bool overMax;
    private bool underMin;

    void Update()
    {
        if (device == null)
        {
            device = SteamVR_Controller.Input((int)trackedObject.index);
        }

        if (device != null && snapTurning && device.GetAxis().x != 0)
        {
            float xAxis = device.GetAxis().x;

            if (xAxis > snapPoint && overMax == false)
            {
                overMax = true;
                playerTransform.transform.RotateAround(playerHeadTransform.position, Vector3.up, 15);
            }
            else if (xAxis < snapPoint && overMax == true)
            {
                overMax = false;
            }

            if (xAxis < -snapPoint && underMin == false)
            {
                underMin = true;
                playerTransform.transform.RotateAround(playerHeadTransform.position, Vector3.up, -15);
            }
            else if (xAxis > -snapPoint && underMin == true)
            {
                underMin = false;
            }
        }
    }
}

