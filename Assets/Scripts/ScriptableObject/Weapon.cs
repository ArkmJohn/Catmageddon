using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject {

    [Tooltip("For Unnetworked Testing")]
    public GameObject ammo;

    [Tooltip("For the Resource Folder Object Name in Networked Play")]
    public string ammoName;

    [Tooltip("Damage to be given by the Character class")]
    public float damage;
    public float ProjectileSpeed = 500f;
    public ParticleSystem fireParticle;

    public abstract void ArmWeapon(GameObject spawnPoint, GameObject myTank, PunTeams.Team myTeam);
}
