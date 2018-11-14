using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class ModelAutomatedSetup : MonoBehaviour 
{
    private BoxCollider boxCollider;

    private Rigidbody rb;

    private Character character;
    private Model model;

    private SelectionIndicator selectionIndicator;

    private PhotonView photonView;
    private PhotonTransformView photonTransformView;
    private ModelInteraction_NET modelInteraction;

    public void AddMissingModelScripts()
    {
        boxCollider = GetOrAddComponent<BoxCollider>();

        rb = GetOrAddComponent<Rigidbody>();

        character = GetOrAddComponent<Character>();

        model = GetOrAddComponent<Model>();

        selectionIndicator = GetOrAddComponent<SelectionIndicator>();

        photonView = GetOrAddComponent<PhotonView>();

        photonTransformView = GetOrAddComponent<PhotonTransformView>();

        modelInteraction = GetOrAddComponent<ModelInteraction_NET>();
    }

    private T GetOrAddComponent<T>() where T : Component
    {
        if (GetComponent<T>())
        {
            return GetComponent<T>();
        }
        else
        {
            return gameObject.AddComponent<T>() as T;
        }
    }

    public void PopulateScripts()
    {
        // box collider needs to be sized manually

        model.objectName = character.characterData.characterName;

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        selectionIndicator.highlightMaterial = (Material)Resources.Load("SelectHighlight_MAT");

        photonView.OwnershipTransfer = OwnershipOption.Takeover;
        photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
        while (photonView.ObservedComponents.Count > 1)
        {
            photonView.ObservedComponents.RemoveAt(1);
        }
        photonView.ObservedComponents[0] = photonTransformView;

        photonTransformView.m_SynchronizeScale = true;
    }

    public void RemoveComponents()
    {
        Component[] components = GetComponents(typeof(Component));

        for (int i = components.Length-1; i > 0; i--)
        {
            if (components[i] != GetComponent<Character>() && components[i] != this && components[i] != GetComponent<BoxCollider>() && components[i] != transform)
            {
                if (components[i] != null)
                {
                    Component componentToDestroy = components[i];
                    DestroyImmediate(componentToDestroy, true);
                }

                components[i] = null;
            }
        }
    }
}
