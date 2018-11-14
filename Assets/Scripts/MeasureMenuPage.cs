using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeasureMenuPage : MenuPageBase
{
    public TextMeshProUGUI measureDistanceText;

    public void SetMeasureDistanceText(float distanceBetweenPoints)
    {
        string distanceDisplayText = distanceBetweenPoints.ToString("f1") + " ft";

        measureDistanceText.text = distanceDisplayText;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        measureDistanceText.text = "";
    }
}
