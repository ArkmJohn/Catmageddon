using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPowerUp : PowerUpEffectApplication {

	public float speedIncrease;

	//Power Up Behaviour
	//Several spawn around the map in random locations
	//Short respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Temporaily increases player's speed stat for it's duration

	public override void ApplyPowerUpEffect () 
	{
		//Speed Function
		GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
		Destroy (this.gameObject);
	}
}
