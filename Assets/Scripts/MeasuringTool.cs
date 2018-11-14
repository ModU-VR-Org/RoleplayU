using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeasuringTool : MonoBehaviour, IInitializable
{
    public GameObject measureToolLinePrefab;
    public float yOffset = 0.05f;

    private Vector3 clickedPosition = Vector3.zero;
    private Vector3 lastClickedPosition = Vector3.zero;
    
    private MeasureMenuPage measureMenuPage;

    private GameObject measureLine;
    private LineRenderer measureLineRenderer;
    private MeasureLine measureLineScript;

    private InputManager inputManager;
    private LaserPointer laser;

    public void Initialize()
    {
        inputManager = GetComponentInParent<MenuManager>().inputManager;
        laser = inputManager.gameObject.GetComponentInChildren<LaserPointer>(true);

        measureMenuPage = GetComponent<MeasureMenuPage>();
    }

    public void OnEnable()
    {
        inputManager.controllerRight.OnTriggerClicked += TriggerClicked;
    }

    private void OnDisable()
    {
        inputManager.controllerRight.OnTriggerClicked -= TriggerClicked;
        if (measureLineScript != null)
        {
            measureLineScript.UpdateMeasureLineRpcCall(Vector3.zero, Vector3.zero, false);
        }
    }

    private void TriggerClicked()
    {
        if (laser.tagOfCurrentTarget != "MaintainSelection")
        {
            if (measureLine == null)
            {
                SpawnMeasureLine();
            }
            if (measureLine.activeInHierarchy == false)
            {
                // reset last clicked position to the player's position
                Vector3 playerPosition = new Vector3(transform.position.x, 0, transform.position.z);
                lastClickedPosition = playerPosition;

                measureLine.SetActive(true);
            }

            clickedPosition = laser.hitPosition + new Vector3(0, yOffset, 0);
            measureLineScript.UpdateMeasureLineRpcCall(lastClickedPosition, clickedPosition, true);

            lastClickedPosition = clickedPosition;

            float distanceBetweenPoints = Vector3.Distance(measureLineRenderer.GetPosition(0), measureLineRenderer.GetPosition(1)) * 3.281f; //meters converted to feet
            measureMenuPage.SetMeasureDistanceText(distanceBetweenPoints);
        }
    }

    private void SpawnMeasureLine()
    {
        measureLine = PhotonNetwork.Instantiate(measureToolLinePrefab.name, Vector3.zero, Quaternion.identity, 0);

        measureLineRenderer = measureLine.GetComponent<LineRenderer>();
        measureLineScript = measureLine.GetComponent<MeasureLine>();
    }
}
