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
	int[] limits  = new int[4];
	public int[] spawned  = new int[4];
	int randomLocationID;
	int tracker;
	string type;
	Vector3 spawnOffset;
	bool canSpawn;
	bool respawnAvail;


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
		canSpawn = false;
		respawnAvail = false;
		tracker = 0;
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
	bool SpawnCheck (int powerUpNum)
	{
		if (spawned [powerUpNum] != limits [powerUpNum]) 
		{
			Debug.Log ("Hit from here");
			return true;
		} 
		else 
		{
			return false;
		}
	}

	bool RespawnCheck (int powerUpNum)
	{
		if (respawn [powerUpNum] != respawnTimers [powerUpNum]) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}


	void SetUp (int powerUpNum)
	{
		randomLocationID = (int)Random.Range (0.0f, spawnLocations.Length);
		//defines power up being spawned
		if (powerUpNum == 0) 
		{
			type = "Catnip";
		}
		else if (powerUpNum == 1) 
		{
			type = "Milk";
		}
		else if (powerUpNum == 2) 
		{
			type = "Tuna";
		}
		else if (powerUpNum == 3) 
		{
			type = "Cheese";
		}
		//calls the spawn method
		Spawn (randomLocationID, type);
		//increments spawned count, deduction will be done in actual power up
		spawned[powerUpNum] ++;
		//resets respawn timer
		respawn[powerUpNum] = 0;
	}

	void Update()
	{
		for(int j = 0; j <= spawned.Length-1; j++)
		{
			Debug.Log ("started this shit");

			canSpawn = SpawnCheck (tracker);
			if(canSpawn == true)
			{
				respawn[tracker] ++;
				respawnAvail = RespawnCheck (tracker);
				if (respawnAvail == true) 
				{
					SetUp (tracker);
				}
			}
			tracker++;
		}
		tracker = 0;
	}
}