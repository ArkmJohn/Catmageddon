using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheesePowerUp : MonoBehaviour {

	bool active;
	string teamType;
	//float duration;
	int powerUpID;

	//Power Up Behaviour
	//Spawns in the middle of map
	//On player pickup/touch despawns/deactivates and hides
	//Spawns rats/mice for player's team that touched it

	// Use this for initialization
	void Start () 
	{
		powerUpID = 3;
		active = false;
	}

	void OnCollisionEnter(Collision player)
	{
		//Check if colliding object is a player
		//Check player's team 
		//Spawn rats for that player's team
		//Deduct amount from purrmanager
		//Destroy this object

		if (player.gameObject.tag == "Player") 
		{
			//Get TeamID From Player Class
			//Spawn Mice for Players Team via AISpawner
			//teamType = "";
			active = true;
			GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpManager> ().PowerUpPickedUp (powerUpID);
			Destroy (this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
