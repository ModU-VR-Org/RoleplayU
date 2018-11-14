//======= Copyright (c) Valve Corporation, All rights reserved. ===============

// lightly modified version of SteamVR_LaserPointer, for use in this project

using UnityEngine;
using System.Collections;

public struct PointerEventArgs
{
    public uint controllerIndex;
    public uint flags;
    public float distance;
    public Transform target;
}

public delegate void PointerEventHandler(object sender, PointerEventArgs e);

public class LaserPointer : MonoBehaviour
{
    public bool active = true;
    public Color color;
    public float defaultThickness = 0.002f;
    private float currentThickness;
    public GameObject holder;
    public GameObject pointer;
    bool isActive = false;
    public bool addRigidBody = false;
    public Transform reference;
    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;
    public LayerMask laserMask;

    public string tagOfCurrentTarget;

    [HideInInspector]
    public Vector3 hitPosition;

    Transform previousContact = null;

    public InputManager inputManager;

    public Transform pointerDirection;

    private float raycastMaxDistance = 100f;

    // Use this for initialization
    void Start ()
    {
        holder = new GameObject();
        holder.transform.parent = transform;

        //holder.transform.localPosition = Vector3.zero;
        //holder.transform.localPosition = new Vector3(0.022f, -0.04f, 0);
        //holder.transform.localRotation = Quaternion.identity;
        //holder.transform.localRotation = Quaternion.Euler(25.0f, -4.0f, 0);

        holder.transform.localPosition = pointerDirection.localPosition;
        holder.transform.localRotation = pointerDirection.localRotation;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(defaultThickness, defaultThickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		pointer.transform.localRotation = Quaternion.identity;
		BoxCollider collider = pointer.GetComponent<BoxCollider>();
        if (addRigidBody)
        {
            if (collider)
            {
                collider.isTrigger = true;
            }
            Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
        else
        {
            if(collider)
            {
                Destroy(collider);
            }
        }
        //Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        //newMaterial.SetColor("_Color", color);
        //pointer.GetComponent<MeshRenderer>().material = newMaterial;
        pointer.GetComponent<MeshRenderer>().material.color = Color.cyan;


        //inputManager = GetComponentInParent<InputManager>();
        inputManager.controllerRight.OnTriggerClicked += TriggerClicked;
        inputManager.controllerRight.OnTriggerUnClicked += TriggerUnClicked;

        currentThickness = defaultThickness;
    }


    public virtual void OnPointerIn(PointerEventArgs e)
    {
        if (PointerIn != null)
            PointerIn(this, e);
    }

    public virtual void OnPointerOut(PointerEventArgs e)
    {
        if (PointerOut != null)
            PointerOut(this, e);
    }

    private void TriggerClicked()
    {
        currentThickness = defaultThickness * 5;
    }

    private void TriggerUnClicked()
    {
        currentThickness = defaultThickness;
    }

    void Update ()
    {
        if (!isActive)
        {
            isActive = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        float dist = 100f;

        Ray raycast = new Ray(pointerDirection.position, pointerDirection.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit, raycastMaxDistance, laserMask);

        // Tracking laser hit position
        hitPosition = hit.point;

        if (previousContact && previousContact != hit.transform)
        {
            PointerEventArgs args = new PointerEventArgs();
            args.distance = 0f;
            args.flags = 0;
            args.target = previousContact;
            OnPointerOut(args);
            previousContact = null;
        }
        if(bHit && previousContact != hit.transform)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = hit.transform;
            OnPointerIn(argsIn);
            previousContact = hit.transform;

            tagOfCurrentTarget = hit.transform.tag;
        }
        if(!bHit)
        {
            previousContact = null;
        }
        if (bHit && hit.distance < raycastMaxDistance)
        {
            dist = hit.distance;
        }

        pointer.transform.localScale = new Vector3(currentThickness, currentThickness, dist);
        pointer.transform.localPosition = new Vector3(0f, 0f, dist/2f);
    }
}
