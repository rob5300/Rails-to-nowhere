﻿using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{

    public Transform player;
    public Transform playerCam;
    public float throwForce = 10;
    bool hasPlayer = false;
    bool beingCarried = false;
    private bool touched = false;
	private float _timeSinceCarry = 0f;

    void Start()
    {
 
    }

    void Update()
    {
		_timeSinceCarry += Time.deltaTime;
        float dist = Vector3.Distance(gameObject.transform.position, player.position); //checks distance between game object and player
        if (dist <= 5f)
        {
            hasPlayer = true; //if distance is less or equal to 2.5, object can be picked up
        }
        else
        {
            hasPlayer = false;
        }
        if (hasPlayer && Input.GetMouseButtonDown(1)) //if the object can be picked up, it will follow the camera
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            beingCarried = true;
        }
		else if (beingCarried && _timeSinceCarry > 1f && Input.GetMouseButtonDown(1))
		{
			GetComponent<Rigidbody>().isKinematic = false;
			transform.parent = null;
			beingCarried = false;
		}
        //if (beingCarried) -- what is this doing?
        //{
        //    if (touched)
        //    {
        //        GetComponent<Rigidbody>().isKinematic = false;
        //        transform.parent = null;
        //        beingCarried = false;
        //        touched = false;
        //    }
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        GetComponent<Rigidbody>().isKinematic = false;
        //        transform.parent = null;
        //        beingCarried = false;
        //        GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
       
        //    }
        //    else if (Input.GetMouseButtonDown(1))
        //    {
        //        GetComponent<Rigidbody>().isKinematic = false;
        //        transform.parent = null;
        //        beingCarried = false;
        //    }
        //}
    }

    void OnTriggerEnter() //if the object touches something else, it falls down
    {
        if (beingCarried)
        {
            touched = true;
        }
    }
}