using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDiceButton : MonoBehaviour
{
    public int diceType = 4;
    public RollTool rollTool;
	
	public void Activate()
    {
        rollTool.ChangeSelectedRoll(diceType, gameObject);
    }
}
