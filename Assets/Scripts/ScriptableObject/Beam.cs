using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catmageddon/Weapon-Types/Beam")]
public class Beam : Weapon
{
    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank, PunTeams.Team myTeam)
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
        bulletClone.GetComponent<WeaponCollider>().Damage = damage;
        bulletClone.GetComponent<WeaponCollider>().team = myTeam;
        Physics.IgnoreCollision(bulletClone.GetComponentInChildren<Collider>(), myTank.GetComponent<Collider>());
        //myPView.photonView.RPC("UseWeapon", PhotonTargets.All, spawnPoint, myTank, TeamId);

    }

    [PunRPC]
    void UseWeapon(GameObject spawnPoint, GameObject myTank)
    {
        GameObject bulletClone;
        bulletClone = Instantiate(ammo, spawnPoint.transform.position, Quaternion.identity);
        bulletClone.transform.SetParent(spawnPoint.transform);
        bulletClone.GetComponent<WeaponCollider>().Damage = damage;
        Physics.IgnoreCollision(bulletClone.GetComponentInChildren<Collider>(), myTank.GetComponent<Collider>());
    }
}
