using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagCapture : MonoBehaviour {

	private CaptureController captureController;
    public bool IsCaptured = false; // SERIALIZE PLS
    public PunTeams.Team CaptureTeam = PunTeams.Team.none; // SERIALIZE PLS
    public bool IsBeingCaptured = false; // SERIALIZE PLS
    public MeshRenderer flagMat; // Materials of the flag material
    public Dictionary<PunTeams.Team, float> PercentageList = new Dictionary<PunTeams.Team, float>();

    // Use this for initialization
    void Start () 
	{
        flagMat.material.color = Color.white;
        captureController = GameObject.Find("CaptureCollider").GetComponent<CaptureController> ();
	}

    void Update()
    {

    }

	void OnTriggerStay (Collider other)
	{
        if (CaptureTeam != other.GetComponent<CharInfo>().MyTeam)
        {

        }


	}

    void CaptureThis(PunTeams.Team team)
    {

    }
	/*void OnTriggerEnter (Collider other) 
	{
		
		if (other.gameObject.tag == "BlueTeam")  
		{
		
			captureController.BlueTeam = true;

		}

		if (other.gameObject.tag == "RedTeam") 
		{
			captureController.RedTeam = true;
		}

	} */

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
