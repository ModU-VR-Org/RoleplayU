using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickUIElements : MonoBehaviour
{

    private Transform currentSelection = null;
    private Transform lastClicked = null;

    public PlayerController playerController;


    void Start()
    {
        GetComponent<SteamVR_TrackedController>().TriggerClicked += OnTriggerClicked;
        LaserPointer laser = GetComponent<LaserPointer>();
        if (laser != null)
        {
            laser.PointerIn += OnPointerIn;
            laser.PointerOut += OnPointerOut;
        }
        else
        {
            Debug.Log("ERROR! No laser pointer found");
        }
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        currentSelection = e.target;
    }

    void OnPointerOut(object sender, PointerEventArgs e)
    {
        currentSelection = null;
    }

    void OnTriggerClicked(object sender, ClickedEventArgs args)
    {
        //    if (currentSelection != null)
        //    {
        //        if (currentSelection.CompareTag("UI"))
        //        {
        //            currentSelection.GetComponent<Button>().onClick.Invoke();
        //        }
        //        else
        //        {
        //            playerController.DeselectModel();
        //        }

        //        if (currentSelection.CompareTag("Model"))
        //        {
        //            playerController.SelectModel(currentSelection.gameObject);
        //        }
        //    }
        //    else //No Object Selected by Click
        //    {
        //        if (playerController != null)
        //        {
        //            playerController.DeselectModel();
        //        }
        //    }
        //}
    }
}
