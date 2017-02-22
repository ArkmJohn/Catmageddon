using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : ScriptableObject
{
    public GameObject ammo;
    public float damage;
    public ParticleSystem fireParticle;

    public abstract void ArmWeapon(GameObject spawnPoint, GameObject myTank);
}
