using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagCapture : MonoBehaviour {

	private CaptureController captureController = GetComponent<CaptureController>();

	// Use this for initialization
	void Start () {

		CaptureController = GameObject.Find("Player").GetComponent<CaptureController>;
	}

	void OnTriggerEnter () {
		
		if (Collision.gameObject.tag == "BlueTeam")  {
			CaptureController.BlueTeam = true;

		}

		if (Collision.gameObject.tag == "RedTeam") {
				CaptureController.RedTeam = true;

			}

	}

				void OnTriggerExit () {

		if (Collision.gameObject.CompareTag == "BlueTeam") {
						CaptureController.BlueTeam = false;

					}

		if (Collision.gameObject.CompareTag == "RedTeam") {
							CaptureController.RedTeam = false;

						}

							}
	
}
