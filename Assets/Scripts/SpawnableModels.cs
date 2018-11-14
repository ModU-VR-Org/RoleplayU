using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnableModels : ScriptableObject 
{   
    [SerializeField]
    public List<GameObject> prefabList; //objects in this list are prefabs added in the inspector
}
