using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenController : MonoBehaviour 
{
	void Start () 
	{
        gameObject.SetActive(true);
        Invoke("DisableBlackScreen", 1.8f);
	}
	
	private void DisableBlackScreen()
    {
        gameObject.SetActive(false);
    }
}
