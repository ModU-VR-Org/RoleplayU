using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AvatarArtAndCharacterData : ScriptableObject 
{
    public List<GameObject> characterPrefabsList; //objects in this list are prefabs added in the inspector
    [System.Serializable]
    public class HandObject
    {
        public GameObject handLeft;
        public GameObject handRight;
    }
    [SerializeField]
    public List<HandObject> characterPrefabHandsList; //must be set in same order as "characterPrefabsList" to match

}
