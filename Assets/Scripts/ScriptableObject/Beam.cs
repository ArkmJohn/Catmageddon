using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catmageddon/Weapon-Types/Beam")]
public class Beam : Weapon
{
    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        GameObject bulletClone;

        if (PhotonNetwork.connected == false) // For Unnetworked Testing
        {
            bulletClone = Instantiate(ammo, spawnPoint.transform.position, Quaternion.identity);
        }
        else // For Networked Play
        {
            bulletClone = PhotonNetwork.Instantiate(ammoName, spawnPoint.transform.position, Quaternion.identity, 0);
        }

        bulletClone.transform.SetParent(spawnPoint.transform);
        
        Physics.IgnoreCollision(bulletClone.GetComponentInChildren<Collider>(), myTank.GetComponent<Collider>());
    }
}
