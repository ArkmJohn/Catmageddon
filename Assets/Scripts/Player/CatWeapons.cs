using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CatInfo))]
public class CatWeapons : Photon.PunBehaviour, IPunObservable
{

    [Tooltip("Child Destination for the SpawnPoint of the Bullets")]
    public string spawnPointDestination = "Turret/Gun/Spawn";
    [Tooltip("for the SpawnPoint of the Bullets")]
    public GameObject projectileSpawnTransform;

    TankObject myTank;
    [SerializeField]
    GameObject beamWeapon; // Gets filled when the tank has the beam firemode

    float MainWeaponCooldown;
    [SerializeField]
    bool beamActive = false;
    bool hasSetup = false;

    #region Unity Callbacks

	// Update is called once per frame
	void Update ()
    {
        if (hasSetup && beamWeapon)
        {
            if (beamActive && beamWeapon != null)
            {
                beamWeapon.SetActive(true);
            }
            else if (beamWeapon.activeSelf && !beamActive)
            {
                beamWeapon.SetActive(false);
            }
        }
        if (hasSetup)
            UseMyWeapons();
    }

    #endregion

    #region Public Methods

    public void SetUpWeapons(GameObject tank, TankObject _myTank)
    {
        myTank = _myTank;
        projectileSpawnTransform = tank.transform.Find(spawnPointDestination).gameObject;
        MainWeaponCooldown = myTank.MainCoolDown;
        if (myTank.MainWeaponFireMode == FireMode.Beam)
        {
            myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject);
            beamWeapon = projectileSpawnTransform.transform.GetChild(0).gameObject;
            beamWeapon.SetActive(false);
        }
        hasSetup = true;
    }

    public void UseMyWeapons()
    {
        if (!PhotonNetwork.connected)
        {
            if (Input.GetButton("Fire1"))
            {
                myTank.SideWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject);
            }
            ControlMainWeapon();
        }
        else if (photonView.isMine)
        {
            if (Input.GetButton("Fire1"))
            {
                myTank.SideWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject);
            }
            ControlMainWeapon();
        }
    }

    #endregion

    #region Private Methods

    void ControlMainWeapon()
    {
        if (MainWeaponCooldown > 0)
            MainWeaponCooldown -= Time.deltaTime;
        if (MainWeaponCooldown <= 0)
            MainWeaponCooldown = 0;

        switch (myTank.MainWeaponFireMode)
        {
            case FireMode.SemiAutomatic:
                if (Input.GetButtonDown("Fire2"))
                {
                    // If we can fire and the cd is below zero
                    if (MainWeaponCooldown <= 0)
                    {
                        myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject);
                        MainWeaponCooldown = myTank.MainCoolDown;
                    }
                    else
                        Debug.Log("Weapon Still On Cooldown");
                }
                break;

            case FireMode.Automatic:
                if (Input.GetButton("Fire2"))
                {
                    myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject);
                }
                break;

            case FireMode.Beam:
                if (Input.GetButton("Fire2"))
                {
                    BeamUse(true);
                }
                else
                {
                    BeamUse(false);
                }
                break;
        }

    }

    void BeamUse(bool state)
    {
        beamActive = state;
    }

    #endregion

    #region IpunObservable Methods

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(beamActive);
        }
        else
        {
            // Network player, receive data
            this.beamActive = (bool)stream.ReceiveNext();
        }

    }

    #endregion
}
