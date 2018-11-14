using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResizeMenuPage : MenuPageBase, IInitializable
{
    public ResizeTool resizeTool;
    public TextMeshProUGUI nameOfModelToResize;
    public TextMeshProUGUI xAxisValue;
    public TextMeshProUGUI yAxisValue;
    public TextMeshProUGUI zAxisValue;

    private GameObject selectedModel;

    public void Initialize()
    {
        menuManager.selectionTool.OnSelection += SetSelectedModel;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        menuManager.SetScrollButtonsActive(false);
    }

    private void SetSelectedModel(GameObject obj)
    {
        selectedModel = obj;

        if (selectedModel != null)
        {
            nameOfModelToResize.text = selectedModel.GetComponent<Model>().objectName;
            Transform resizeModelTransform = selectedModel.transform;
            xAxisValue.text = resizeModelTransform.localScale.x.ToString("F2");
            yAxisValue.text = resizeModelTransform.localScale.y.ToString("F2");
            zAxisValue.text = resizeModelTransform.localScale.z.ToString("F2");
        }
        else
        {
            nameOfModelToResize.text = "select an object...";
            xAxisValue.text = "-";
            yAxisValue.text = "-";
            zAxisValue.text = "-";
        }
    }

    public void ResizeAxisIncrease(int axisId)
    {
        resizeTool.ResizeAxis(selectedModel, axisId, 1.0f);
        SetResizeMenuText();
    }
    public void ResizeAxisDecrease(int axisId)
    {
        resizeTool.ResizeAxis(selectedModel, axisId, -1.0f);
        SetResizeMenuText();
    }

    public void SetResizeMenuText()
    {
        if (selectedModel != null)
        {
            Transform resizeModelTransform = selectedModel.transform;
            xAxisValue.text = resizeModelTransform.localScale.x.ToString("F2");
            yAxisValue.text = resizeModelTransform.localScale.y.ToString("F2");
            zAxisValue.text = resizeModelTransform.localScale.z.ToString("F2");
        }
    }
}
