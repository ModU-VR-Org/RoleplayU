using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHoverList : MonoBehaviour
{
    public List<GameObject> objectsHoveringOver = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        objectsHoveringOver.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        while (objectsHoveringOver.Contains(other.gameObject))
        {
            objectsHoveringOver.Remove(other.gameObject);
        }
    }

    //private void Update()
    //{
    //    foreach (GameObject obj in objectsHoveringOver)
    //    {
    //        if (obj.activeInHierarchy == false)
    //        {
    //            objectsHoveringOver.Remove(obj);
    //        }
    //    }
    //}
}
