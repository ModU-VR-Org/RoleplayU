using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script can be attached to a PlayerRig to fake movement
//if you don't have enough testers for all your VR headset connections

public class FakePlayerBot : MonoBehaviour {

    public Transform mainTransform;
    public Transform leftHand;
    public Transform rightHand;
    public Transform head;
    public bool isFakePlayer = false;

	void Start () {		
	}
	void Update () {

        if (Input.GetButtonDown("Jump"))
        {
            if (!isFakePlayer)
            {
                isFakePlayer = true;
            }
            else
            {
                isFakePlayer = false;
            }
        }

        if (isFakePlayer)
        {
            mainTransform.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.1f), Random.Range(-0.2f, 0.5f)) * Time.deltaTime;
            leftHand.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0, 0.1f), Random.Range(-0.2f, 0.5f)) * Time.deltaTime;
            rightHand.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0, 0.1f), Random.Range(-0.2f, 0.5f)) * Time.deltaTime;
            head.position += new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(0, 0.1f), Random.Range(-0.2f, 0.5f)) * Time.deltaTime;
        }		
	}
}
