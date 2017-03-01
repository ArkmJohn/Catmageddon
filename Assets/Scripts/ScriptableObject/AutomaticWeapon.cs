using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catmageddon/Weapon-Types/Automatic Weapon")]
public class AutomaticWeapon : Weapon
{

    public float AttackCooldown = 0.5f;
    public float ProjectileSpeed = 500f;
    float myCoolDown;

    public void OnEnable()
    {
        myCoolDown = AttackCooldown;
    }

    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        myCoolDown -= Time.deltaTime;

        if (myCoolDown <= 0)
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
            myCoolDown = AttackCooldown;
        }

    }

}
