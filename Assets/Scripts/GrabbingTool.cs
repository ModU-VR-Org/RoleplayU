using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GrabbingTool : MonoBehaviour
{
    private SelectionTool selectionTool;
    private InputManager inputManager;

    private GameObject selectedModel;
    private GameObject grabbedModel;

    private IGrabbable grabbable;

    public class GrabbingHand
    {
        public Transform controllerTransform;
        public bool left;
        public bool isGrabbing;
        public bool isGrabbingModel;

    }
    public GrabbingHand leftHand;
    public GrabbingHand rightHand;
    private float grabRadius = 0.08f; 

    void Start()
    {
        rightHand = new GrabbingHand();
        leftHand = new GrabbingHand();

        selectionTool = GetComponent<SelectionTool>();
        selectionTool.OnSelection += SetSelectedModel;

        inputManager = GetComponent<InputManager>();
        inputManager.controllerRight.OnGripClicked += GripClickedRight;
        inputManager.controllerRight.OnGripUnClicked += GripUnClickedRight;
        inputManager.controllerLeft.OnGripClicked += GripClickedLeft;
        inputManager.controllerLeft.OnGripUnClicked += GripUnClickedLeft;

        rightHand.controllerTransform = inputManager.controllerRight.controller.transform;
        leftHand.controllerTransform = inputManager.controllerLeft.controller.transform;
        leftHand.left = true;
        rightHand.left = false;
    }

    private void SetSelectedModel(GameObject obj)
    {
        selectedModel = obj;

        if (leftHand.isGrabbing == true && grabbedModel != null)
        {
            UnGrabModel(grabbedModel, leftHand);
        }

        if (rightHand.isGrabbing == true && grabbedModel != null)
        {
            UnGrabModel(grabbedModel, rightHand);
        }
    }

    private void GripClickedRight()
    {
        Grab(rightHand);
    }

    private void GripUnClickedRight()
    {
        UnGrab(rightHand);
    }

    private void GripClickedLeft()
    {
        Grab(leftHand);
    }

    private void GripUnClickedLeft()
    {
        UnGrab(leftHand);
    }

    private void Grab(GrabbingHand hand)
    {
        if (hand.isGrabbing == false)
        {            
            GrabHandColliderObject(hand);            
        }

        if (hand.isGrabbing == false && selectedModel != null)
        {
            GrabModel(hand);
        }
    }

    private void GrabHandColliderObject(GrabbingHand hand)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hand.controllerTransform.position, grabRadius);
        foreach (Collider col in hitColliders)
        {
            IGrabbable collisionScript = col.GetComponent<IGrabbable>();
            if(collisionScript != null)
            {
                bool grabbed = collisionScript.Grab(hand);
                if (grabbed == true)
                {
                    hand.isGrabbing = true;
                    return;
                }
            }
        }
    }

    private void GrabModel(GrabbingHand hand)
    {        
        grabbable = selectedModel.GetComponent<IGrabbable>();
        if (grabbable != null)
        {
            hand.isGrabbingModel = grabbable.Grab(hand);
            if (hand.isGrabbingModel)
            {
                grabbedModel = selectedModel;
                hand.isGrabbing = true;
            }
        }
    }

    private void UnGrab(GrabbingHand hand)
    {
       UnGrabHandColliderObject(hand);

        if (hand.isGrabbingModel)
        {
            UnGrabModel(selectedModel, hand);
        }
    }

    private void UnGrabHandColliderObject(GrabbingHand hand)
    {
        if (hand.isGrabbing)
        {
            Collider[] hitColliders = Physics.OverlapSphere(hand.controllerTransform.position, grabRadius);

            foreach (Collider col in hitColliders)
            {
                IGrabbable collisionScript = col.GetComponent<IGrabbable>();
                if(collisionScript != null)
                {
                    collisionScript.UnGrab();
                }
            }
            hand.isGrabbing = false;
        }
    }

    private void UnGrabModel(GameObject model, GrabbingHand hand)
    {
        if ((grabbable = model.GetComponent<IGrabbable>()) != null)
        {
            grabbable.UnGrab();
        }

        hand.isGrabbingModel = false;
        hand.isGrabbing = false;
        grabbedModel = null;
    }
}