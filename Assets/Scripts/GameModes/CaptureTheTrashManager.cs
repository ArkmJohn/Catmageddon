using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTheTrashManager : MonoBehaviour 
{
	//Blue Team ID is 0 and Red Team is 1
	public GameObject redTrashCan;
	public GameObject blueTrashCan;
	public GameObject redBaseSpawnLocation;
	public GameObject blueBaseSpawnLocation; 

	int delaySpawner;
	float blueReturnTimer;
	float redReturnTimer;
	string trashIdentifier;
	Vector3 pickUpOffset;
	bool intialSpawn;
	bool isBlueDropped;
	bool isRedDropped;
	bool isDelivered;

	// Use this for initialization
	void Start () 
	{
		delaySpawner = 20;
		intialSpawn = false;
		isBlueDropped = false;
		isRedDropped = false;
		isDelivered = false;

	}

	//Function to Spawn TrashCan at their bases on game start when Manager is created
	void SpawnTrashCans()
	{
		Instantiate(redTrashCan, redBaseSpawnLocation.transform.position, Quaternion.identity);
		Instantiate(blueTrashCan, blueBaseSpawnLocation.transform.position, Quaternion.identity);
		intialSpawn = true;
	}

	//Resets Trash Cans to spawn  location after return timer has counted down
	void ResetToSpawn(string trashCan)
	{
		if (trashCan == "blueTrashCan") 
		{
			isBlueDropped = false;
			blueReturnTimer = 0.0f;
			blueTrashCan.transform.position = blueBaseSpawnLocation.transform.position;
		}
		else if (trashCan == "redTrashCan")
		{
			isRedDropped = false;
			redReturnTimer = 0.0f;
			redTrashCan.transform.position = redBaseSpawnLocation.transform.position;
		}
	}


	//Triggers when Trash Can reaches enemy base, increases a teams score and resets all values
	//and trashcan to base
	public void ConfirmDelivery(string trashID)
	{
		if (trashID == "blueTrashCan") 
		{
			blueTrashCan.transform.parent = null;
			blueTrashCan.SetActive (false);
			//IncreaseScore
			ResetToSpawn (trashID);
			blueTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (false);
			blueTrashCan.SetActive (true);
		}
		else if(trashID == "redTrashCan")
		{
			redTrashCan.transform.parent = null;
			redTrashCan.SetActive (false);
			//IncreaseScore
			ResetToSpawn (trashID);
			redTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (false);
			redTrashCan.SetActive (true);
		}
	}

	//Triggers function when player collides with opposing team's trash can
	//Disables their ability to pick up and binds trash can to that player
	public void PickUpTrashCan(Transform Player, string trashCan)
	{
		if (trashCan == "blueTeam") 
		{
			blueTrashCan.transform.position = Player.position;
			blueTrashCan.transform.parent = Player.transform;
			blueTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (true);
			if (isBlueDropped == true) 
			{
				isBlueDropped = false;
				blueReturnTimer = 0.0f;
			}
		} 
		else if (trashCan == "redTeam") 
		{
			redTrashCan.transform.position = Player.position;
			redTrashCan.transform.parent = Player.transform;
			redTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (true);
			if (isRedDropped == true) 
			{
				isRedDropped = false;
				redReturnTimer = 0.0f;
			}
		}
	}

	//To be triggered when player dies, unbinds trashcan to player and leaves it at the 
	//current position they died at and enables the ability to be picked up and starts
	//a count down timer
	public void DropTrashCan(int playerTeam)
	{
		if (playerTeam == 0) 
		{
			blueTrashCan.transform.parent = null;
			blueTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (false);
			isBlueDropped = true;
		} 
		else if (playerTeam == 1) 
		{
			redTrashCan.transform.parent = null;
			redTrashCan.GetComponent<TrashCanValues> ().SetPickUpState (false);
			isRedDropped = true;
		}
	}
		
	// Update is called once per frame
	void Update () 
	{
		//Initial spawn of the trash cans at the start of update
		if (intialSpawn == false) 
		{
			SpawnTrashCans ();
		}

		//Timer for trash Can reset
		if (isBlueDropped == true) 
		{
			//Add visual Element for the Timers
			blueReturnTimer++;
			if (blueReturnTimer == 30.0f) 
			{
				trashIdentifier = "blueTrashCan";
				ResetToSpawn (trashIdentifier);
			}
		}
		if (isRedDropped == true) 
		{
			redReturnTimer++;
			if (redReturnTimer == 30.0f) 
			{
				trashIdentifier = "redTrashCan";
				ResetToSpawn (trashIdentifier);
			}
		}
	}
}
