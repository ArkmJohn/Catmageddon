﻿using System;
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

    public PunTeams.Team myTeam;

    TankObject myTank;
    [SerializeField]
    GameObject beamWeapon; // Gets filled when the tank has the beam firemode

    float MainWeaponCooldown;
    [SerializeField]
    bool beamActive = false;
    bool hasSetup = false;

    #region Unity Callbacks

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine && this.hasSetup)
        {
            UseMyWeapons();

            if ((PhotonNetwork.player.ID + 1) % 2 == 0)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            }
            else
                PhotonNetwork.player.SetTeam(PunTeams.Team.red);
        }

        if (this.IsFiringSide)
        {
            this.myTank.SideWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, PhotonNetwork.player.GetTeam());
        }

        if (this.beamWeapon == null && this.IsFiringMain)
            this.myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, PhotonNetwork.player.GetTeam());

        if(this.beamWeapon != null && this.IsFiringMain != this.beamWeapon.GetActive())
                this.beamWeapon.SetActive(this.IsFiringMain);
        
        if(PhotonNetwork.player.IsLocal && myTeam == PunTeams.Team.none)
        {
            myTeam = PhotonNetwork.player.GetTeam();
        }
    }

    #endregion

    #region Public Methods

    public void SetUpWeapons(GameObject tank, TankObject _myTank)
    {
        Debug.Log("Setting up weapons");
        myTank = _myTank;
        projectileSpawnTransform = tank.transform.Find(spawnPointDestination).gameObject;
        MainWeaponCooldown = myTank.MainCoolDown;
        if (myTank.MainWeaponFireMode == FireMode.Beam)
        {
            myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, PhotonNetwork.player.GetTeam());
            beamWeapon = projectileSpawnTransform.transform.GetChild(0).gameObject;
            beamWeapon.SetActive(false);
        }
        hasSetup = true;
    }


    public bool IsFiringSide;
    public bool IsFiringMain;

    public void UseMyWeapons()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //myTank.SideWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, GetComponent<CatInfo>().teamID, GetComponent<PhotonView>());
            if (!this.IsFiringSide)
            {
                this.IsFiringSide = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (this.IsFiringSide)
            {
                this.IsFiringSide = false;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            //myTank.SideWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, GetComponent<CatInfo>().teamID, GetComponent<PhotonView>());
            if (!this.IsFiringMain)
            {
                this.IsFiringMain = true;
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (this.IsFiringMain)
            {
                this.IsFiringMain = false;
            }
        }

        //ControlMainWeapon();
    }

    #endregion

    /*
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
                        myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, GetComponent<CatInfo>().teamID, GetComponent<PhotonView>());
                        MainWeaponCooldown = myTank.MainCoolDown;
                    }
                    else
                        Debug.Log("Weapon Still On Cooldown");
                }
                break;

            case FireMode.Automatic:
                if (Input.GetButton("Fire2"))
                {
                    myTank.MainWeapon.ArmWeapon(projectileSpawnTransform, this.gameObject, GetComponent<CatInfo>().teamID, GetComponent<PhotonView>());
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
*/
    #region IpunObservable Methods

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(beamActive);
            stream.SendNext(IsFiringMain);
            stream.SendNext(IsFiringSide);
            stream.SendNext(beamWeapon.GetActive());
        }
        else
        {
            // Network player, receive data
            //this.beamActive = (bool)stream.ReceiveNext();
            this.IsFiringMain = (bool)stream.ReceiveNext();
            this.IsFiringSide = (bool)stream.ReceiveNext();
            this.beamWeapon.SetActive((bool)stream.ReceiveNext());
        }

    }

    #endregion
}
