using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPowerUp : PowerUpEffectApplication {

	//Power Up Behaviour
	//Several spawn around the map in random locations
	//Short respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Temporaily increases player's speed stat for it's duration

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CatInfo>() != null && PhotonNetwork.player.IsLocal)
        {
            other.GetComponent<CatInfo>().GotMilk();
            DestroyMe();
        }
    }
}
