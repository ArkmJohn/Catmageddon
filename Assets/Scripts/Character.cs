using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Photon.PunBehaviour {

    public Tank myTank;
    public GameObject projectileSpawn;

    public float maxHealth;
    public float damageCooldown = 0.2f;

    GameObject beam;

    [Tooltip("The current Health of our player")]
    [SerializeField]
    float health;
    float secCD;
    [SerializeField]
    float dCD;
    [SerializeField]
    bool gotDamaged = false;

    void Awake()
    {
        GameObject tank = Instantiate(myTank.TankPrefab, this.transform.position, Quaternion.identity);
        projectileSpawn = tank.transform.GetChild(0).gameObject;
        secCD = myTank.SecondaryCoolDown;
        tank.transform.SetParent(this.transform);
        maxHealth = myTank.health;
        health = maxHealth;
        dCD = damageCooldown;
        if (myTank.myFireMode == FireMode.Beam)
        {
            myTank.Weapon2.ArmWeapon(projectileSpawn, this.gameObject);
            beam = projectileSpawn.transform.GetChild(0).gameObject;
            beam.SetActive(false);
        }
    }

    public void UseWeapons()
    {
        if (Input.GetButton("Fire1"))
        {
            myTank.Weapon1.ArmWeapon(projectileSpawn, this.gameObject);
        }

        SecondaryWeapon();
    }

    void Update()
    {
        if (gotDamaged)
        {

            if (dCD <= 0)
            {
                gotDamaged = false;
                dCD = damageCooldown;
            }
            else
            {
                dCD -= Time.deltaTime;
            }
        }
    }

    void SecondaryWeapon()
    {
        if (secCD > 0)
            secCD -= Time.deltaTime;
        if (secCD <= 0)
            secCD = 0;

        switch (myTank.myFireMode)
        {
            case FireMode.SingleFire:
                if (Input.GetButtonDown("Fire2") && secCD <= 0)
                {
                    myTank.Weapon2.ArmWeapon(projectileSpawn, this.gameObject);
                    secCD = myTank.SecondaryCoolDown;
                }
                break;

            case FireMode.Automatic:
                if (Input.GetButton("Fire2"))
                {
                    myTank.Weapon2.ArmWeapon(projectileSpawn, this.gameObject);
                }
                break;

            case FireMode.Beam:
                if (Input.GetButton("Fire2"))
                {
                    beam.SetActive(true);
                }
                else
                {
                    beam.SetActive(false);
                }
                break;
        }
    }

    public void GetDamaged( float damage)
    {
        if (!gotDamaged)
        {
            Debug.Log("got Damaged");
            gotDamaged = true;
            //dCD = damageCooldown;
            health -= damage;
            if (health <= 0)
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        Debug.Log("I Died GJ");
    }

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (!other.GetComponent<ProjectileController>())
        {
            return;
        }

        GetDamaged(other.GetComponent<ProjectileController>().damage);

    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (!other.GetComponent<ProjectileController>())
        {
            return;
        }

        GetDamaged(other.GetComponent<ProjectileController>().damage);
    }
    #endregion
}
