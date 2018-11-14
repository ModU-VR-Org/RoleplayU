using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_NetworkTest : MonoBehaviour 
{
    private Transform myTransform;
    private float coolDown = 5f;
    private float coolDownTracker;
    Vector3 randomDirection;
    float speed = 1.3f;
    float turnSpeed = 1.8f;

    void Start () 
	{
        myTransform = GetComponent<Transform>();
	}
	

	void Update () 
	{
        if(coolDownTracker > coolDown)
        {
            coolDownTracker = 0;
            randomDirection = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f));
        }
        coolDownTracker++;
        myTransform.position += randomDirection * speed * Time.deltaTime;

        myTransform.Rotate(Vector3.right * Time.deltaTime * turnSpeed);
        myTransform.Rotate(Vector3.up * Time.deltaTime * turnSpeed);
        myTransform.Rotate(Vector3.forward * Time.deltaTime * turnSpeed);

    }
}
