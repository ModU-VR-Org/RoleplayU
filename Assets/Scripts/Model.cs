using UnityEngine;

public class Model : MonoBehaviour
{
    public string objectName = "";
  
    [HideInInspector] // TODO, this isn't currently being used, just a placeholder for potentially adding this functionality later
    public string tags = ""; //tags for searching, seperated by ", " (aka comma+space)

    [HideInInspector]
    // model Id stores what index the model is in the menu model list, and is used for saving and loading game states. it is populated when the model is spawned. 
    public int modelId;
}
