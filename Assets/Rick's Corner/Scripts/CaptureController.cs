using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureController : MonoBehaviour {

	public float redCapturePercentage = 0;
	public float blueCapturePercentage = 0;
	public bool BlueTeam = false;
	public bool RedTeam = false;
	//private GameObject Flag;
	//public GameObject Mist;
	private MeshRenderer flagMat;

	// Use this for initialization
	void Start () 
	{
		//Flag.GetComponent <Renderer> ().material.color = new Color (
		//Mist.GetComponent <Renderer> ().enabled = true;
		flagMat = GameObject.Find ("Flag").GetComponent<MeshRenderer> ();
		//flagMat.color = Color.white;
		flagMat.material.color = Color.white;
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
			flagMat.material.color = Color.red;
			//Flag.GetComponent <Renderer> ().enabled = true;

			//mistRed.GetComponent<Renderer> ().enabled = true;
			//mistBlue.GetComponent<Renderer> ().enabled = false;
			//mistNeutral.GetComponent<Renderer> ().enabled = false;
		}

		if (blueCapturePercentage >= 100) 
		{

			blueCapturePercentage = 100;
			flagMat.material.color = Color.blue;
			//FlagRed.GetComponent<MeshRenderer> ().enabled = false;
			//FlagBlue.GetComponent <MeshRenderer> ().enabled = true;
			//FlagNeutral.GetComponent<MeshRenderer> ().enabled = false;

			//mistRed.GetComponent <Renderer> ().enabled = false;
			//mistBlue.GetComponent <Renderer> ().enabled = true;
			//mistNeutral.GetComponent<Renderer> ().enabled = false;
		}

		if (blueCapturePercentage <= 50 && redCapturePercentage <= 51) 
		{

			//FlagRed.GetComponent<MeshRenderer> ().enabled = false;
			//FlagBlue.GetComponent<MeshRenderer> ().enabled = false;
			//FlagNeutral.GetComponent<MeshRenderer> ().enabled = true;
			flagMat.material.color = Color.white;
			//mistRed.GetComponent<Renderer> ().enabled = false;
			//mistBlue.GetComponent<Renderer> ().enabled = false;
			//mistNeutral.GetComponent<Renderer> ().enabled = true;
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
				
}
