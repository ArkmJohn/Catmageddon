using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanValues : MonoBehaviour {

	public string teamID;
	bool spawned;
	bool pickedUp;

	// Allows other classes to set the trashcan's pick up state
	public void SetPickUpState (bool newState) 
	{
		pickedUp = newState;
	}
	
	// Allows other classes to get the trashcan's pick up state
	public bool GetPickUpState () 
	{
		return pickedUp;
	}


	//Trigger for when the wtrashcan arrives at the enemny base
	//To Do for John photon.local.host
	void OnTriggerEnter(Collider enemyBase)
	{
		if (enemyBase.gameObject.tag == "redTeamBase") 
		{
			if(teamID == "blueTrashCan")
			{
				GameObject.FindGameObjectWithTag ("TrashCanManager").GetComponent<CaptureTheTrashManager> ().ConfirmDelivery (teamID);
			}
		}
		else if (enemyBase.gameObject.tag == "blueTeamBase") 
		{
			if(teamID == "redTrashCan")
			{
				GameObject.FindGameObjectWithTag ("TrashCanManager").GetComponent<CaptureTheTrashManager> ().ConfirmDelivery (teamID);
			}
		}
	}
}
