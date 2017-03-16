using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunaPickUp : PowerUpEffectApplication {

	public int heal;

	//Power Up Behaviour
	//Spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage


	public override void ApplyPowerUpEffect (int playerID) 
	{
		//Heal Function
		GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
		Destroy (this.gameObject);
	}
}
