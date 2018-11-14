using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTool : MonoBehaviour 
{
    public LaserPointer laser;

    private GameObject pointedAtObject;
    private InputManager inputManager;

    private GameObject selectedObject;

    public delegate void SelectionEventHandler(GameObject obj);
    public event SelectionEventHandler OnSelection; 

    private ISelectable selectable;

    void Start()
    {
        if (laser != null)
        {
            laser.PointerIn += OnPointerIn;
            laser.PointerOut += OnPointerOut;
        }

        inputManager = GetComponent<InputManager>();
        inputManager.controllerRight.OnTriggerClicked += TriggerClicked;
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        pointedAtObject = e.target.gameObject;
    }

    void OnPointerOut(object sender, PointerEventArgs e)
    {
        pointedAtObject = null;
    }

    private void TriggerClicked()
    {
        if (pointedAtObject != null)
        {
            if (pointedAtObject.CompareTag("Model"))
            {
                SelectModel(pointedAtObject);
            }
            else if (pointedAtObject.CompareTag("MaintainSelection") == false)
            {
                DeselectModel();
            }

        }
        else //No Object Selected by Click
        {
            DeselectModel();
        }
    }

    private void SelectModel(GameObject clickedObject)
    {
        DeselectModel();

        if ((selectable = clickedObject.GetComponent<ISelectable>()) != null)
        {
            bool isSelected;

            isSelected = selectable.Select();

            if (isSelected)
            {
                selectedObject = clickedObject;
                RaiseSelectionEvent(selectedObject);
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    public void DeselectModel()
    {
        if (selectedObject != null)
        {
            if ((selectable = selectedObject.GetComponent<ISelectable>()) != null)
            {
                selectable.Deselect();
                SetSelectionToNull();
            }
        }
    }

    public void SetSelectionToNull()
    {
        selectedObject = null;
        RaiseSelectionEvent(null);
    }

    private void RaiseSelectionEvent(GameObject selectObject)
    {
        if (OnSelection != null)
        {
            OnSelection(selectObject);
        }
    }
}
