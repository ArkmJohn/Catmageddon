using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour {

	public float redCapturePercentage = 0;
	public float blueCapturePercentage = 0;
	public bool BlueTeam = false;
	public bool RedTeam = false;
	public GameObject FlagRed;
	public GameObject FlagBlue;
	public GameObject FlagNeutral;
	public GameObject mistRed;
	public GameObject mistBlue;
	public GameObject mistNeutral;
	// Use this for initialization
	void Start () 
	{
		FlagRed.GetComponent<MeshRenderer> ().enabled = false;
		FlagBlue.GetComponent<MeshRenderer> ().enabled = false;
		FlagNeutral.GetComponent<MeshRenderer> ().enabled = true;

		mistRed.GetComponent <Renderer> ().enabled = false;
		mistBlue.GetComponent <Renderer> ().enabled = false;
		mistNeutral.GetComponent <Renderer> ().enabled = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (BlueTeam == true) 
		{
			blueCapturePercentage += Time.deltaTime * 20;
			redCapturePercentage -= Time.deltaTime * 20;

		}

		if (RedTeam == true) 
		{
			redCapturePercentage += Time.deltaTime * 20;
			blueCapturePercentage -= Time.deltaTime * 20;

		}

		if (RedTeam == true && BlueTeam == true) 
		{
			redCapturePercentage = redCapturePercentage;
			blueCapturePercentage = blueCapturePercentage;
		}

		if (redCapturePercentage >= 100) 
		{

			redCapturePercentage = 100;
			FlagRed.GetComponent<MeshRenderer> ().enabled = true;
			FlagBlue.GetComponent<MeshRenderer> ().enabled = false;
			FlagNeutral.GetComponent<MeshRenderer> ().enabled = false;

			mistRed.GetComponent<Renderer> ().enabled = true;
			mistBlue.GetComponent<Renderer> ().enabled = false;
			mistNeutral.GetComponent<Renderer> ().enabled = false;
		}

		if (blueCapturePercentage >= 100) 
		{

			blueCapturePercentage = 100;
			FlagRed.GetComponent<MeshRenderer> ().enabled = false;
			FlagBlue.GetComponent <MeshRenderer> ().enabled = true;
			FlagNeutral.GetComponent<MeshRenderer> ().enabled = false;

			mistRed.GetComponent <Renderer> ().enabled = false;
			mistBlue.GetComponent <Renderer> ().enabled = true;
			mistNeutral.GetComponent<Renderer> ().enabled = false;
		}

		if (blueCapturePercentage <= 50 && redCapturePercentage <= 51) 
		{

			FlagRed.GetComponent<MeshRenderer> ().enabled = false;
			FlagBlue.GetComponent<MeshRenderer> ().enabled = false;
			FlagNeutral.GetComponent<MeshRenderer> ().enabled = true;

			mistRed.GetComponent<Renderer> ().enabled = false;
			mistBlue.GetComponent<Renderer> ().enabled = false;
			mistNeutral.GetComponent<Renderer> ().enabled = true;
		}

		if (redCapturePercentage <= 0) 
		{
			redCapturePercentage = 0;
		}

		if (blueCapturePercentage <= 0) 
		{
			blueCapturePercentage = 0;
		}
	}

//	void OnGUI () 
//	{
//		GUI.Box(Rect(10,10,300,25), "Red Cap" + " " + redCapturePercentage.ToString("0") + " " + "BlueCap" + " " + blueCapturePercentage.ToString("0"));
//	}
}
