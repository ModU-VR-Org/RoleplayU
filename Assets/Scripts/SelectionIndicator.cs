using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour
{
    public Material highlightMaterial;

    private GameObject rendererGameObject;
    private GameObject highlightGameObject;

    private void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            rendererGameObject = renderer.gameObject;
            highlightGameObject = Instantiate(rendererGameObject, rendererGameObject.transform.position, rendererGameObject.transform.rotation);
            highlightGameObject.transform.SetParent(transform);
            highlightGameObject.transform.localScale = rendererGameObject.transform.localScale;
            highlightGameObject.GetComponent<Renderer>().material = highlightMaterial;
            highlightGameObject.SetActive(false);
        }
    }

    public void ToggleHighlight(bool isHighlighted)
    {
        highlightGameObject.SetActive(isHighlighted);
    }
}
