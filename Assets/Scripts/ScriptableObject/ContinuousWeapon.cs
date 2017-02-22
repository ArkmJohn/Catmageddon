using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Catma/Projectile/Continuous Weapon")]
public class ContinuousWeapon : Projectile
{
    public float cooldown = 0.2f;
    public float projectileSpeed = 200;
    float myCCD;

    public void OnEnable()
    {
        myCCD = cooldown;
    }

    public override void ArmWeapon(GameObject spawnPoint, GameObject myTank)
    {
        
        myCCD -= Time.deltaTime;

        if (myCCD <= 0)
        {
            Instantiate(fireParticle, spawnPoint.transform.position, spawnPoint.transform.rotation);
            GameObject fire = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation);
            fire.GetComponent<Rigidbody>().AddForce(spawnPoint.transform.forward * projectileSpeed);
            Physics.IgnoreCollision(fire.GetComponent<Collider>(), myTank.GetComponent<Collider>());
            myCCD = cooldown;
        }
    }
}
