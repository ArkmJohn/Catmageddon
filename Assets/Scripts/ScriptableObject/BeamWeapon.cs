using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catma/Projectile/Beam Weapon")]
public class BeamWeapon : Projectile
{

    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        //Instantiate(fireParticle, spawnPoint.transform.position, spawnPoint.transform.rotation);
        GameObject fire = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation);
        fire.transform.SetParent(spawnPoint.transform);
        Physics.IgnoreCollision(fire.GetComponentInChildren<Collider>(), myTank.GetComponent<Collider>());
    }
}
