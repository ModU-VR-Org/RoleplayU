using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTool : MonoBehaviour 
{
    public float resizeFactor = 0.1f;
    public ResizeMenuPage resizeMenuPage;

    public void ResizeAxis(GameObject modelToResize, int axisId, float postiveOrNegative)
    {
        if (modelToResize != null)
        {
            float scaleMultiplier = 1 + (postiveOrNegative * resizeFactor);
            Vector3 scaleMultiplierVector;

            if (axisId == 1)
                scaleMultiplierVector = new Vector3(scaleMultiplier, 1f, 1f);
            else if (axisId == 2)
                scaleMultiplierVector = new Vector3(1f, scaleMultiplier, 1f);
            else if (axisId == 3)
                scaleMultiplierVector = new Vector3(1f, 1f, scaleMultiplier);
            else
                scaleMultiplierVector = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);

            modelToResize.transform.localScale = Vector3.Scale(modelToResize.transform.localScale, scaleMultiplierVector);
            resizeMenuPage.SetResizeMenuText();
        }
    }
}
