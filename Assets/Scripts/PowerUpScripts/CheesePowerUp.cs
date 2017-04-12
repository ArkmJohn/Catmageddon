using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheesePowerUp : PowerUpEffectApplication {

    //Power Up Behaviour
    //Spawns in the middle of map
    //On player pickup/touch despawns/deactivates and hides
    //Spawns rats/mice for player's team that touched it

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CatInfo>() != null && PhotonNetwork.player.IsLocal)
        {
            // Call PowerUpManager
            if(other.gameObject.GetComponent<CatInfo>().MyTeam == PunTeams.Team.blue)
                FindObjectOfType<NetworkPurrUpMewnager>().SpawnAI("Blue");

            if (other.gameObject.GetComponent<CatInfo>().MyTeam == PunTeams.Team.red)
                FindObjectOfType<NetworkPurrUpMewnager>().SpawnAI("Red");

            DestroyMe();
        }
    }
}
