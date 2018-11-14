using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceLocalPlayer : MonoBehaviour
{
    private Transform localPlayerCamera;
    private Camera[] cameraArray;

    void Start ()
    {
        cameraArray = FindObjectsOfType<Camera>();
        for (int i = 0; i < cameraArray.Length; i++)
        {
            if (cameraArray[i].isActiveAndEnabled)
            {
                localPlayerCamera = cameraArray[i].transform;
            }
        }
    }

    void Update ()
    {
        transform.LookAt(localPlayerCamera);
	}
}
