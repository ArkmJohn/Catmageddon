using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNipPowerUp : PowerUpEffectApplication {
	//Power Up Behaviour
	//One spawns around the map in a random location
	//Slightly longer respawn timer
	//On player pickup/touch despawns/deactivates and hides
	//Checks the player who picked it up
	//Disables the player's ability to take damage

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CatInfo>() != null && PhotonNetwork.player.IsLocal && !other.gameObject.GetComponent<CatInfo>().IsInvunrable)
        {
            other.GetComponent<CatInfo>().GotNip();
            DestroyMe();
        }
    }
}
