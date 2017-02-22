using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNipPowerUp : MonoBehaviour {

	int duration;
	bool active;
	bool invulnerability;
	int powerUpID;

	//Power Up Behaviour
	//One spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage

	// Use this for initialization
	void Start () 
	{
		powerUpID = 0;
		active = false;
		invulnerability = false;
	}

	void OnCollisionEnter(Collision player)
	{
		//Check if colliding object is a player
		//Apply effect to this player
		//make player invulnerable for duration of powerup
		//Deduct amount from purrmanager
		//Destroy this object

		if (player.gameObject.tag == "Player") 
		{
			invulnerability = true;
			//player.gameObject.GetComponent<Player> ().MakeInvulnerable(invulnerability);
			active = true;
			GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
			Destroy (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{

	}
}
