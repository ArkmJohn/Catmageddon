using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPowerUp : MonoBehaviour {

	bool active;
	int powerUpID;
	float speedIncrease;

	//Power Up Behaviour
	//Several spawn around the map in random locations
	//Short respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Temporaily increases player's speed stat for it's duration

	// Use this for initialization
	void Start () 
	{
		powerUpID = 1;
		active = false;
		speedIncrease = 50.0f;
	}

	void OnCollisionEnter(Collision player)
	{
		//Check if colliding object is a player
		//Apply effect to this player
		//make player faster for duration of powerup
		//Deduct amount from purrmanager
		//Destroy this object

		if (player.gameObject.tag == "Player") 
		{
			//player.gameObject.GetComponent<Tank> ().IncreaseSpeed(speedIncrease);
			active = true;
			GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
			Destroy (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
