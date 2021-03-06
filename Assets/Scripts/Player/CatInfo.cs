﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CatInfo : CharInfo, IPunObservable
{
    #region Public Variables

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("Health Fill Image that wil be used for health indicator")]
    public Image HealthContent;
    [Tooltip("This Players Name Text UI")]
    public Text NameText;
    [Tooltip("This is the Gameobject to be activated if the Catnip is picked up")]
    public GameObject CatNipEffects;

    [Tooltip("The Firemode of the CurrentPlayer")]
    public FireMode MyFireMode;

    [Tooltip("The speed the Player will move")]
    public float MySpeed;
    [Tooltip("The Duration of the MilkSpeed Length")]
    public float MilkSpeedLength = 5;
    [Tooltip("The Speed to be added when Milk is ingested")]
    public float MilkSpeedAdd = 10;
    [Tooltip("The Duration of Invunrability")]
    public float CatnipLength = 3;


    #endregion

    #region Private Variables

    float BaseSpeed;

    // True when Side or Main is Firing
    bool IsFiringSide;
    bool IsFiringMain;

    // Cooldows for the weapons
    float maxCDSide = 0.5f;
    float CDSide = 0.5f;
    float maxCDMain = 0.5f;
    float CDMain = 0.5f;

    // Durations for the Powerups
    float MilkSpeedDur = 5;
    float CatNipDur = 3;
    // Bool for Milk
    bool HasMilk = false;

    [SerializeField]
    int myCatID = 1;
    [SerializeField]
    int myHatID = 1;
    [SerializeField]
    int myTankID = 1;
    string MyName;

    // Tank Object for the current player
    TankObject MyTankObject;

    // Beam Weapon if the player has a beam weapon
    public GameObject Beam = null;
    GameObject SideWeaponPrefab = null; // Side Weapon Prefab Holder
    GameObject MainWeaponPrefab = null; // Main Weapon Prefab if the player is not beam type firemode

    // Projectile Spawn Point
    GameObject ProjectileSpawnPoint;
    #endregion

    #region MonoBehavior CallBacks
    void Awake()
    {
        if (photonView.isMine)
        {
            LocalPlayerInstance = gameObject;
        }
    }

    void Start()
    {

        if (photonView.isMine)
        {
            MyTeam = PhotonNetwork.player.GetTeam();
            // Set Up Tank
            this.myTankID = PhotonNetwork.player.GetTank();
            this.myHatID = PhotonNetwork.player.GetHat();
            this.myCatID = PhotonNetwork.player.GetCat();
            this.MyName = PhotonNetwork.player.NickName;
            this.MyTeam = PhotonNetwork.player.GetTeam();
            this.photonView.RPC("SetupTank", PhotonTargets.All, PhotonNetwork.player.GetTank(),
                PhotonNetwork.player.GetHat(), PhotonNetwork.player.GetCat());

            //SetupTank(myTankID, myHatID, myCatID);
        }
        //SetupTank(myTankID, myHatID, myCatID);
        //this.photonView.RPC("SetupTank", PhotonTargets.All, myTankID, myHatID, myCatID);

    }

    void Update()
    {
        // Checks if the Current Game State is Over
        if (GameManager.CurrentGameState == GameState.OVER)
        {
            return;
        }

        if (photonView.isMine)
        {
            if (!IsDead())
            {
                if (this.GetComponent<CameraControl>().localPlayer == null)
                {
                    this.GetComponent<CameraControl>().localPlayer = this.transform;
                }
                this.ProcessInput();
            }
            else
            {
                this.photonView.RPC("Die", PhotonTargets.All);
                FindObjectOfType<GameManager>().PlayerDied(PhotonNetwork.player);
                ResetPlayer();
            }
        }

        CDSide -= Time.deltaTime;
        CDMain -= Time.deltaTime;

        if (MyTankObject != null)
        {
            this.BaseSpeed = MyTankObject.Speed;
        }

        if (HealthContent != null)
        {
            if (!IsDead())
            {
                float temp = this.Health / this.maxHealth;
                //Debug.Log(temp);
                HealthContent.fillAmount = temp;
                HealthContent.color = Color.Lerp(Color.red, Color.green, Mathf.PingPong(temp, 1));
            }
            else
                HealthContent.fillAmount = 0;
        }
        if (hasSetup)
        {
            if (this.TankBody != null && !this.hasSetupWeapons)
            {
                this.photonView.RPC("SetUpWeapons", PhotonTargets.All);
                return;
            }

            if (this.MyFireMode == FireMode.Beam && this.IsFiringMain != this.Beam.GetActive()) // Calls the Main Weapons
            {
                if (this.IsFiringMain == true)
                {
                    AudioManager.instance.PlayRandomVolWithSrc(MyTankObject.MainWeapon.attackSound, this.GetComponent<AudioSource>());
                }
                this.Beam.SetActive(this.IsFiringMain);
            }
            if (this.MyFireMode != FireMode.Beam && this.MainWeaponPrefab != null && this.IsFiringMain) // Calls the main weapon if not beam
            {
                if (CDMain <= 0)
                {
                    //UseMainWeapon("Main");
                    this.photonView.RPC("UseMainWeapon", PhotonTargets.All, "Main");
                    CDMain = maxCDMain;
                }
            }
        }

        // Activates the effect for the Catnip
        if (this.CatNipEffects != null && this.IsInvunrable != this.CatNipEffects.GetActive())
        {
            this.CatNipEffects.SetActive(this.IsInvunrable);

            if (photonView.isMine)
            {
                CatNipDur -= Time.deltaTime;
                if (CatNipDur <= 0)
                {
                    CatNipDur = CatnipLength;
                    CatNipDur = CatnipLength;
                    this.IsInvunrable = false;

                }
            }

        }

        // Use or Activate Side weapons
        if (this.SideWeaponPrefab != null && this.IsFiringSide)
        {
            if (CDSide <= 0)
            {
                //UseSideWeapon("Side");
                this.photonView.RPC("UseSideWeapon", PhotonTargets.All, "Side");
                CDSide = maxCDSide;
            }
        }

        // Update The Name of the Player
        if (this.NameText != null && this.NameText.text != this.MyName)
            this.NameText.text = this.MyName;

        // Update the color of the Vehicle
        if (hasChangeColor == false && MyTeam != PunTeams.Team.none)
        {
            UpdateColors();
        }

        if (this.HasMilk)
        {
            MilkSpeedDur -= Time.deltaTime;
            this.MySpeed = this.BaseSpeed + this.MilkSpeedAdd;
            if (MilkSpeedDur <= 0)
            {
                MilkSpeedDur = MilkSpeedLength;
                this.HasMilk = false;

            }
        }
        else
        {
            this.MySpeed = this.BaseSpeed;
        }


    }

    void OnCollisionEnter(Collision other)
    {
        // We dont do anything if we're not the local player
        if (!photonView.isMine)
        {
            return;
        }

        if (other.gameObject.tag != "Weapon")
        {
            return;
        }

        //this.Health -= other.gameObject.GetComponent<WeaponCollider>().Damage;
    }

    void OnTriggerStay(Collider other)
    {
        // We dont do anything if we're not the local player
        if (!photonView.isMine)
        {
            return;
        }

        if (other.GetComponent<Flag>() != null)
        {
            if (this.MyTeam == PunTeams.Team.red)
            {
                //other.GetComponent<Flag>().RedTeamCapture();

                PhotonView pView = other.GetComponent<PhotonView>();
                pView.photonView.RPC("RedTeamCapture", PhotonTargets.All);
            }
            else
            {
                //other.GetComponent<Flag>().BlueTeamCapture();

                PhotonView pView = other.GetComponent<PhotonView>();
                pView.photonView.RPC("BlueTeamCapture", PhotonTargets.All);
            }
        }

        if (other.gameObject.tag != "Weapon")
        {
            return;
        }

        this.Health -= other.GetComponent<WeaponCollider>().Damage * Time.deltaTime;
    }

    void OnTriggerExit(Collider other)
    {
        // We dont do anything if we're not the local player
        if (!photonView.isMine)
        {
            return;
        }
        if (other.GetComponent<Flag>() != null)
        {
            if (this.MyTeam == PunTeams.Team.red)
            {
                //other.GetComponent<Flag>().RunAwayRedFromCollider();

                PhotonView pView = other.GetComponent<PhotonView>();
                pView.photonView.RPC("RunAwayRedFromCollider", PhotonTargets.All);
            }
            else
            {
                //other.GetComponent<Flag>().RunAwayBlueFromCollider();

                PhotonView pView = other.GetComponent<PhotonView>();
                pView.photonView.RPC("RunAwayBlueFromCollider", PhotonTargets.All);
            }
        }
    }

    #endregion

    #region Public Methods

    public void GotNip()
    {
        this.IsInvunrable = true;
    }

    public void GotMilk()
    {
        this.HasMilk = true;
    }

    [PunRPC]
    public void Die()
    {
        Instantiate(Resources.Load("Diesplosion"), this.transform.position, this.transform.rotation);
    }

    public void UseMainWeapon()
    {
        if (this.MyFireMode == FireMode.Beam) // Calls the Main Weapons
        {

            AudioManager.instance.PlayRandomVolWithSrc(MyTankObject.MainWeapon.attackSound, this.GetComponent<AudioSource>());
            this.Beam.SetActive(true);
        }
        if (this.MyFireMode != FireMode.Beam && this.MainWeaponPrefab != null) // Calls the main weapon if not beam
        {
            if (CDMain <= 0)
            {
                //UseMainWeapon("Main");
                this.photonView.RPC("UseMainWeapon", PhotonTargets.All, "Main");
                CDMain = maxCDMain;
            }
        }
    }

    public void UseSideWeapon()
    {
        // Use or Activate Side weapons
        if (this.SideWeaponPrefab != null)
        {
            if (CDSide <= 0)
            {
                //UseSideWeapon("Side");
                this.photonView.RPC("UseSideWeapon", PhotonTargets.All, "Side");
                CDSide = maxCDSide;
            }
        }
    }

    #endregion

    #region Private Methods

    public bool hasSetup = false;

    [PunRPC]
    void SetupTank(int tankIndex, int hatID, int catID)
    {
        TankObject myTank = GameManager.Instance.TankPrefabs[tankIndex];
        MyTankObject = myTank;
        // Spawns the tank Object
        GameObject Tank;
        //GameObject Tank = PhotonNetwork.Instantiate(MyTankObject.TankPrefab.name, this.transform.position, this.transform.rotation, 0);
        Tank = Instantiate(Resources.Load(MyTankObject.TankPrefab.name, typeof(GameObject)), this.transform.position, this.transform.rotation) as GameObject;
        Tank.transform.SetParent(this.transform);
        this.TankObject = Tank;
        // Calls the change of color based on the Team
        ChangeColor(Tank.transform.FindChild("Body/Body").gameObject);

        //Set up Cat
        GameObject Cat;
        //GameObject Cat = PhotonNetwork.Instantiate("1" + myCatID.ToString(), this.transform.position, this.transform.rotation, 0);
        Cat = Instantiate(Resources.Load("1" + catID.ToString(), typeof(GameObject)), this.transform.position, this.transform.rotation) as GameObject;
        Cat.transform.SetParent(Tank.transform.FindChild("Turret/CatSpawn"));
        Cat.transform.localPosition = Vector3.zero;
        Cat.transform.localRotation = new Quaternion(0, 0, 0, 0);
        Cat.transform.localScale = Vector3.one;

        // Set up Hat
        GameObject Hat;
        //GameObject Hat = PhotonNetwork.Instantiate("0" + myHatID.ToString(), this.transform.position, this.transform.rotation, 0);
        Hat = Instantiate(Resources.Load("0" + hatID.ToString(), typeof(GameObject)), this.transform.position, this.transform.rotation) as GameObject;
        Hat.transform.SetParent(Tank.transform.FindChild("Turret/CatSpawn/HatSpawn"));
        Hat.transform.localPosition = Vector3.zero;
        Hat.transform.localRotation = new Quaternion(0, 0, 0, 0);
        Hat.transform.localScale = Vector3.one;

        // Sets the Health
        this.maxHealth = myTank.Health;
        this.Health = maxHealth;

        //this.MyFireMode = MyTankObject.MainWeaponFireMode;
        this.photonView.RPC("SetFireMode", PhotonTargets.All, MyTankObject.MainWeaponFireMode);
        //this.photonView.RPC("SetUpWeapons", PhotonTargets.All);
        hasSetup = true;
    }

    public GameObject TankObject;
    public GameObject TankBody;
    public int indexOfMaterialToChange;
    bool hasChangeColor = false;

    // Changes Material
    void ChangeColor(GameObject materialContainer)
    {
        TankBody = materialContainer;
        Material[] mats = materialContainer.GetComponent<Renderer>().materials;

        int indexToChange = 0;

        // Checks if the material is same as the needed to change material
        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i] == GameManager.Instance.TeamMaterial)
            {
                indexToChange = i;
            }
        }

        indexOfMaterialToChange = indexToChange;

    }

    void UpdateColors()
    {
        Material[] mats = this.TankBody.GetComponent<Renderer>().materials;


        if (MyTeam == PunTeams.Team.red)
        {
            mats[indexOfMaterialToChange] = GameManager.Instance.RedMaterial;
        }
        if (MyTeam == PunTeams.Team.blue)
        {
            mats[indexOfMaterialToChange] = GameManager.Instance.BlueMaterial;
        }

        TankBody.GetComponent<Renderer>().materials = mats;
        hasChangeColor = true;
    }

    [PunRPC]
    void SetFireMode(FireMode myFireMode)
    {
        this.MyFireMode = myFireMode;
    }

    public bool hasSetupWeapons = false;

    [PunRPC]
    void SetUpWeapons()
    {
        Debug.Log("Setting Up Weapons");
        this.ProjectileSpawnPoint = TankObject.transform.Find("Turret/Spawn").gameObject;

        this.SideWeaponPrefab = this.MyTankObject.SideWeapon.ammo;

        if (this.MyFireMode == FireMode.Beam)
        {
            GameObject beamObj = PhotonNetwork.Instantiate(MyTankObject.MainWeapon.ammo.name, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
            this.Beam = beamObj;
            this.Beam.GetComponent<WeaponCollider>().team = this.MyTeam;
            this.photonView.RPC("SetBeamToSpawn", PhotonTargets.All);
            this.Beam.SetActive(false);
        }
        else
        {
            this.MainWeaponPrefab = this.MyTankObject.MainWeapon.ammo;
        }
        hasSetupWeapons = true;
    }

    [PunRPC]
    void SetBeamToSpawn()
    {
        this.Beam.transform.SetParent(ProjectileSpawnPoint.transform);
    }

    void ProcessInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))//Input.GetButtonDown("Fire1"))
        {
            if (!this.IsFiringSide)
            {
                this.IsFiringSide = true;
            }
        }
        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            if (this.IsFiringSide)
            {
                this.IsFiringSide = false;
            }
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire2"))
        {
            if (!this.IsFiringMain)
            {
                this.IsFiringMain = true;
            }
        }
        if (CrossPlatformInputManager.GetButtonUp("Fire2"))
        {
            if (this.IsFiringMain)
            {
                this.IsFiringMain = false;
            }
        }
    }

    void FireSide()
    {

    }

    [PunRPC]
    void UseSideWeapon(string message)
    {
        //if (!photonView.isMine)
        //    return;

        //GameObject bulletClone = PhotonNetwork.Instantiate(SideWeaponPrefab.name, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
        GameObject bulletClone = Instantiate(SideWeaponPrefab, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
        Vector3 direction = ProjectileSpawnPoint.transform.forward;
        AudioManager.instance.PlayRandomVolWithSrc(MyTankObject.SideWeapon.attackSound, this.GetComponent<AudioSource>());
        bulletClone.GetComponent<WeaponCollider>().team = this.MyTeam;
        bulletClone.GetComponent<WeaponCollider>().Damage = this.MyTankObject.SideWeapon.damage;

        bulletClone.GetComponent<Rigidbody>().AddForce(direction * MyTankObject.SideWeapon.ProjectileSpeed);
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), this.GetComponent<Collider>());
    }

    [PunRPC]
    void UseMainWeapon(string message)
    {
        //if (!photonView.isMine)
        //    return;

        //GameObject bulletClone = PhotonNetwork.Instantiate(MainWeaponPrefab.name, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
        GameObject bulletClone = Instantiate(MainWeaponPrefab, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
        Vector3 direction = ProjectileSpawnPoint.transform.forward;
        AudioManager.instance.PlayRandomVolWithSrc(MyTankObject.MainWeapon.attackSound, this.GetComponent<AudioSource>());
        bulletClone.GetComponent<WeaponCollider>().team = this.MyTeam;
        bulletClone.GetComponent<WeaponCollider>().Damage = this.MyTankObject.MainWeapon.damage;

        bulletClone.GetComponent<Rigidbody>().AddForce(direction * MyTankObject.MainWeapon.ProjectileSpeed);
        Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), this.GetComponent<Collider>());
    }

    void ResetPlayer()
    {
        // set Transform
        this.transform.position = FindObjectOfType<GameManager>().SpawnPoints[PhotonNetwork.player.ID - 1].transform.position;
        this.transform.rotation = FindObjectOfType<GameManager>().SpawnPoints[PhotonNetwork.player.ID - 1].transform.rotation;

        Health = maxHealth;

    }

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.IsFiringMain);
            stream.SendNext(this.IsFiringSide);
            stream.SendNext(this.MyFireMode);
            stream.SendNext(this.MySpeed);
            stream.SendNext(this.IsInvunrable);
            stream.SendNext(this.HasMilk);
            stream.SendNext(this.MyTeam);
            stream.SendNext(this.Health);
            stream.SendNext(this.CDMain);
            stream.SendNext(this.CDSide);
            stream.SendNext(this.MyName);
            stream.SendNext(this.myTankID);
            stream.SendNext(this.myCatID);
            stream.SendNext(this.myHatID);
        }
        else
        {
            // Network player, receive data
            this.IsFiringMain = (bool)stream.ReceiveNext();
            this.IsFiringSide = (bool)stream.ReceiveNext();
            this.MyFireMode = (FireMode)stream.ReceiveNext();
            this.MySpeed = (float)stream.ReceiveNext();
            this.IsInvunrable = (bool)stream.ReceiveNext();
            this.HasMilk = (bool)stream.ReceiveNext();
            this.MyTeam = (PunTeams.Team)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            this.CDMain = (float)stream.ReceiveNext();
            this.CDSide = (float)stream.ReceiveNext();
            this.MyName = (string)stream.ReceiveNext();
            this.myTankID = (int)stream.ReceiveNext();
            this.myCatID = (int)stream.ReceiveNext();
            this.myHatID = (int)stream.ReceiveNext();
        }
    }

    #endregion
}