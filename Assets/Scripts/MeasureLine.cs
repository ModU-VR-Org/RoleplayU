using UnityEngine;
using TMPro;
using Photon.Pun;

public class MeasureLine : MonoBehaviourPun
{
    public float displayHeight = 1.5f;
    public Vector3 lineOffset = new Vector3(0, 0.1f, 0);
    public TextMeshProUGUI distanceDisplayText;
    public LineRenderer lineRenderer;
    public Transform distanceDisplayTransform;
    public LineRenderer displayPointerLineRenderer;

    public void UpdateMeasureLineRpcCall(Vector3 positionOne, Vector3 positionTwo, bool activeStatus)
    {
        photonView.RPC("UpdateMeasureLine", RpcTarget.All, positionOne, positionTwo, activeStatus);
    }

    [PunRPC]
    private void UpdateMeasureLine(Vector3 positionOne, Vector3 positionTwo, bool activeStatus)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, positionOne + lineOffset);
        lineRenderer.SetPosition(1, positionTwo + lineOffset);
        UpdateDistanceDisplay();
        gameObject.SetActive(activeStatus);
    }

    private void UpdateDistanceDisplay()
    {
        float distanceBetweenPoints = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)) * 3.281f; //meters converted to feet
        distanceDisplayText.text = distanceBetweenPoints.ToString("f1") + " ft";

        Vector3 measureLineMidPoint = Vector3.Lerp(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), 0.5f);
        Vector3 displayPosition = measureLineMidPoint + new Vector3(0, displayHeight, 0);
        distanceDisplayTransform.position = displayPosition;

        displayPointerLineRenderer.SetPosition(0, displayPosition - lineOffset);
        displayPointerLineRenderer.SetPosition(1, measureLineMidPoint + 2 * lineOffset);
    }
}
