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
    //Heal the player for a specific amount of hitpoints

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CatInfo>() != null && PhotonNetwork.player.IsLocal)
        {
            other.GetComponent<CatInfo>().TakeDamage(-heal);
            DestroyMe();
        }
    }

}
