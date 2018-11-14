using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionToken : MonoBehaviour { //make this a static class?

    public static string sessionToken;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
		
	}
	
	
}
