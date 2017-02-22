using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNipPowerUp : PowerUpEffectApplication {

	public bool invulnerability;

	//Power Up Behaviour
	//One spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage

	// Use this for initialization

	public override void ApplyPowerUpEffect () 
	{
		//Invulnerable
		GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
		Destroy (this.gameObject);
	}
}
