using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnvironmentHolder : ScriptableObject
{
    public List<Enviro> environments; //objects in this list are prefab script references added in the inspector
}
