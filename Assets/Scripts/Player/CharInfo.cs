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


}
