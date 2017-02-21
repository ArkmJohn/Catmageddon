using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunaPickUp : MonoBehaviour {

	bool active;
	int powerUpID;
	int heal;

	//Power Up Behaviour
	//Spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage

	// Use this for initialization
	void Start () 
	{
		powerUpID = 2;
		active = false;
		heal = 50;
	}

	void OnCollisionEnter(Collision player)
	{
		//Check if colliding object is a player
		//Apply effect to this player
		//heal player for set amount
		//Deduct amount from purrmanager
		//Destroy this object

		if (player.gameObject.tag == "Player") 
		{
			//player.gameObject.GetComponent<Player> ().HealDamage(heal);
			active = true;
			GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpManager> ().PowerUpPickedUp (powerUpID);
			Destroy (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
