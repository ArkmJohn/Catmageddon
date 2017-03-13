﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagCapture : MonoBehaviour {

	private CaptureController captureController;

	// Use this for initialization
	void Start () 
	{

		captureController = GameObject.Find("Player").GetComponent<CaptureController> ();
	}

	void OnTriggerEnter (Collider other) 
	{
		
		if (other.gameObject.tag == "BlueTeam")  
		{
			captureController.BlueTeam = true;

		}

		if (other.gameObject.tag == "RedTeam") 
		{
			captureController.RedTeam = true;
		}

	}

	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject.tag == "BlueTeam") 
		{
			captureController.BlueTeam = false;
		}

		if (other.gameObject.tag == "RedTeam") 
		{
			captureController.RedTeam = false;
		}

	}
	
}
