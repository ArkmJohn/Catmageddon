using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catmageddon/Weapon-Types/Semi-Automatic Weapon")]
public class SemiAutomaticWeapon : Weapon
{
    [Tooltip("Speed of the Bullet when fired")]
    public float ProjectileSpeed = 500f;

    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        Instantiate(fireParticle, spawnPoint.transform.position, spawnPoint.transform.rotation);
        GameObject bulletClone;

        if (PhotonNetwork.connected == false) // For Unnetworked Testing
        {
            bulletClone = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else // For Networked Play
        {
            bulletClone = PhotonNetwork.Instantiate(ammoName, spawnPoint.transform.position, spawnPoint.transform.rotation, 0);
        }
        Vector3 direction = spawnPoint.transform.forward;

        bulletClone.GetComponent<Rigidbody>().AddForce(direction * ProjectileSpeed);
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), myTank.GetComponent<Collider>());
    }

}
