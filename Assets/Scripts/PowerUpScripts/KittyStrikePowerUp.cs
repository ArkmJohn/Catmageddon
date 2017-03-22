using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KittyStrikePowerUp : PowerUpEffectApplication {

	public string teamType;

	//Power Up Behaviour
	//Spawns in the middle of map
	//On player pickup/touch despawns/deactivates and hides
	//Spawns rats/mice for player's team that touched it

	// Use this for initialization

	public override void ApplyPowerUpEffect (int playerID) 
	{
		//DropDaBomb
		GameObject.FindGameObjectWithTag ("PowerUp Manager").GetComponent<PurrUpMewnager> ().PowerUpPickedUp (powerUpID);
		Destroy (this.gameObject);
	}
}
