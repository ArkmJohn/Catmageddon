using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurrUpMewnager : MonoBehaviour {

	//BasePowerUp Class which abilities are inherited from based on type
	public GameObject[] spawnLocations;
	public GameObject centreSpawnLocation;
	public GameObject Catnip;
	public GameObject Milk;
	public GameObject Tuna;
	public GameObject Cheese;

	//Arrays store the governing values for spawning, 0 is Catnip, 1 is Milk, 2 is Tuna and 3 is Cheese
	int[] respawnTimers  = new int[4];
	int[] respawn = new int[4];
	//int[] durations  = new int[4];
	int[] limits  = new int[4];
	public int[] spawned  = new int[4];
	int randomLocationID;
	string type;
	Vector3 spawnOffset;


	//Power Up Behaviour
	//One spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage

	// Use this for initialization
	void Start () 
	{
		spawnLocations = GameObject.FindGameObjectsWithTag ("PickUpSpawner");
		respawnTimers [0] = 45; respawnTimers [1] = 30; respawnTimers [2] = 15; respawnTimers [3] = 130;
		for (int i = 0; i < respawn.Length; i++) 
		{
			respawn [i] = 0;
		}
		limits [0] = 1; limits [1] = 3; limits [2] = 5; limits [3] = 1;
		for (int i = 0; i < spawned.Length; i++) 
		{
			spawned [i] = 0;
		}
		spawnOffset = new Vector3 (0.0f, 0.5f, 0.0f);
	}

	//Spawns the appropriate power up
	void Spawn(int ID, string type)
	{
		if (type == "Catnip") 
		{
			Instantiate (Catnip, spawnLocations [ID].transform.position + spawnOffset, Quaternion.identity);
		}
		else if (type == "Milk") 
		{
			Instantiate (Milk, spawnLocations [ID].transform.position + spawnOffset, Quaternion.identity);
		}
		else if (type == "Tuna") 
		{
			Instantiate (Tuna, spawnLocations [ID].transform.position + spawnOffset, Quaternion.identity);
		}
		else if (type == "Cheese") 
		{
			Instantiate (Cheese, centreSpawnLocation.transform.position + spawnOffset, Quaternion.identity);
		}
	}

	public void PowerUpPickedUp(int powerUpID)
	{
		spawned [powerUpID]--;
	}


	// Update is called once per frame
	void Update () 
	{
		//Check whether the number of spawned power ups is at the limit
		//If spawned<limit count down respawn timer
		//when timer hits cap spawn appropriate powerup and reset timer

		//Catnip Spawner
		if (spawned[0] != limits[0]) 
		{
			respawn[0] ++;
			if(respawn[0] == respawnTimers[0])
			{
				//need to add in detect if powerup has already spawned at spawner
				//randomises spawn location
				randomLocationID = (int)Random.Range (0.0f, spawnLocations.Length);
				//defines power up being spawned
				type = "Catnip";
				//calls the spawn method
				Spawn (randomLocationID, type);
				//increments spawned count, deduction will be done in actual power up
				spawned[0] ++;
				//resets respawn timer
				respawn[0] = 0;
			}
		}
		//Milk Spawner
		if (spawned[1] != limits[1]) 
		{
			respawn[1] ++;
			if(respawn[1] == respawnTimers[1])
			{
				//need to add in detect if powerup has already spawned at spawner
				randomLocationID = (int)Random.Range (0.0f, spawnLocations.Length);
				type = "Milk";
				Spawn (randomLocationID, type);
				spawned[1] ++;
				respawn[1] = 0;
			}
		}
		//Tuna Spawner
		if (spawned[2] != limits[2]) 
		{
			respawn[2] ++;
			if(respawn[2] == respawnTimers[2])
			{
				//need to add in detect if powerup has already spawned at spawner
				randomLocationID = (int)Random.Range (0.0f, spawnLocations.Length);
				type = "Tuna";
				Spawn (randomLocationID, type);
				spawned[2] ++;
				respawn[2] = 0;
			}
		}
		//Cheese Spawner
		if (spawned[3] != limits[3]) 
		{
			respawn[3] ++;
			if(respawn[3] == respawnTimers[3])
			{
				//Doesn't need a random location
				randomLocationID = 0;
				type = "Cheese";
				Spawn (randomLocationID, type);
				spawned[3] ++;
				respawn[3] = 0;
			}
		}
	}
}