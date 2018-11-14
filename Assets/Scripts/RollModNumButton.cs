using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollModNumButton : MonoBehaviour
{
    public int number;
    public RollTool rollTool;

	public void Activate()
    {
        rollTool.ChangeSelectedModNum(number, gameObject);
    }
}
