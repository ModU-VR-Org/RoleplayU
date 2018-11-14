using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for the note tool keyboard
//To give 2-handed control over keyboard, logic needs to be added to PlayerController

public class KeyboardCollider : MonoBehaviour {

    public PlayerController playerController;

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("HandL")) 
        {
            //TODOTEST playerController.insideKeyboardCollider = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("HandL"))
        {
            //TODOTEST playerController.insideKeyboardCollider = false;
        }
    }
}
