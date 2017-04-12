using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : Photon.PunBehaviour {

    [Tooltip("Max Health of the Character")]
    public float maxHealth;
    [Tooltip("Current Health of the Character")]
    public float Health;
    [Tooltip("Current Team this Character is in")]
    public PunTeams.Team MyTeam;
    [Tooltip("The Bool to check if the player is Invunrable")]
    public bool IsInvunrable = false;

    [PunRPC]
    public void TakeDamage(float DamageValue, PunTeams.Team DamageDealer)
    {
        if (IsInvunrable)
            return;

        if (this.MyTeam == DamageDealer)
            return;

        this.Health -= DamageValue;
    }

    public bool IsDead()
    {
        if (this.Health <= 0)
            return true;
        else
            return false;
    }
}
