using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public int modelIndex;
    public SpawnTool spawnTool;

    public void Spawn()
    {
        spawnTool.SpawnLocalPreview(modelIndex, transform.position);
    }	
}
