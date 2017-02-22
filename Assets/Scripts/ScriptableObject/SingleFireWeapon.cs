using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catma/Projectile/Single-Fire Weapon")]
public class SingleFireWeapon : Projectile
{
    public float projectileSpeed = 200;

    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        Instantiate(fireParticle, spawnPoint.transform.position, spawnPoint.transform.rotation);
        GameObject fire = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation);
        fire.GetComponent<Rigidbody>().AddForce(spawnPoint.transform.forward * projectileSpeed);
        Physics.IgnoreCollision(fire.GetComponent<Collider>(), myTank.GetComponent<Collider>());
    }
}