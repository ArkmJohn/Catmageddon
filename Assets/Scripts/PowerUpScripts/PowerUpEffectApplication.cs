using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffectApplication : MonoBehaviour {

	//Handles PowerUp Effect Distribution
	//Handles duration

	public int powerUpID;

    [PunRPC]
	public abstract void ApplyPowerUpEffect (int playerID) ;
}
