using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatInfo : Photon.PunBehaviour, IPunObservable
{
    #region Public Variables

    [Tooltip("Max Health of the Player")]
    public float maxHealth;
    [Tooltip("Current Health of the Player")]
    public float Health;
    [Tooltip("Current Team this player is in")]
    public PunTeams.Team MyTeam;
    [Tooltip("Health Fill Image that wil be used for health indicator")]
    public Image HealthContent;
    [Tooltip("This Players Name Text UI")]
    public Text NameText;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("The Firemode of the CurrentPlayer")]
    public FireMode MyFireMode;
    
    #endregion

    #region Private Variables

    // True when Side or Main is Firing
    bool IsFiringSide;
    bool IsFiringMain;

    // Cooldows for the weapons
    float maxCDSide = 0.5f;
    float CDSide = 0.5f;
    float maxCDMain = 0.5f;
    float CDMain = 0.5f;

    int myTankID;
    string MyName;

    // Tank Object for the current player
    TankObject MyTankObject;

    // Beam Weapon if the player has a beam weapon
    GameObject Beam = null;
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

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        if (photonView.isMine)
        {
            MyTeam = PhotonNetwork.player.GetTeam();
            // Set Up Tank
            this.myTankID = PhotonNetwork.player.GetTank();
            this.MyName = PhotonNetwork.player.NickName;
        }
        
        SetupTank(myTankID);


    }

    void Update()
    {
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
                FindObjectOfType<GameManager>().PlayerDied(PhotonNetwork.player);
                ResetPlayer();
            }
        }

        CDSide -= Time.deltaTime;
        CDMain -= Time.deltaTime;

        if (HealthContent != null)
        {
            if (!IsDead())
            {
                float temp = this.Health / this.maxHealth;
                Debug.Log(temp);
                HealthContent.fillAmount = temp;
            }
            else
                HealthContent.fillAmount = 0;
        }

        if (this.Beam != null && this.IsFiringMain != this.Beam.GetActive()) // Calls the Main Weapons
        {
            this.Beam.SetActive(this.IsFiringMain);
        }
        if (this.Beam == null && this.MainWeaponPrefab != null && this.IsFiringMain) // Calls the main weapon if not beam
        {
            if (CDMain <= 0)
            {
                //UseMainWeapon("Main");
                this.photonView.RPC("UseMainWeapon", PhotonTargets.All, "Main");
                CDMain = maxCDMain;
            }
        }
        if (this.SideWeaponPrefab != null && this.IsFiringSide)
        {
            if (CDSide <= 0)
            {
                //UseSideWeapon("Side");
                this.photonView.RPC("UseSideWeapon", PhotonTargets.All, "Side");
                CDSide = maxCDSide;
            }
        }

        if (this.NameText != null && this.NameText.text != this.MyName)
            this.NameText.text = this.MyName;
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

        if (other.gameObject.tag != "Weapon")
        {
            return;
        }

        this.Health -= other.GetComponent<WeaponCollider>().Damage * Time.deltaTime;
    }
    #endregion

    #region Public Methods

    [PunRPC]
    public void TakeDamage(float DamageValue)
    {
        this.Health -= DamageValue;
    }

    public bool IsDead()
    {
        if (this.Health <= 0)
            return true;
        else
            return false;
    }

    #endregion
    
    #region Private Methods

    void SetupTank(int TankIndex)
    {
        TankObject myTank = GameManager.Instance.TankPrefabs[TankIndex];
        MyTankObject = myTank;
        // Spawns the tank Object
        GameObject Tank = PhotonNetwork.Instantiate(MyTankObject.TankPrefab.name, this.transform.position, this.transform.rotation, 0);
        Tank.transform.SetParent(this.transform);

        // Sets the Health
        this.maxHealth = myTank.Health;
        this.Health = maxHealth;

        this.MyFireMode = MyTankObject.MainWeaponFireMode;
        SetUpWeapons(Tank);
    }

    void SetUpWeapons(GameObject Tank)
    {
        Debug.Log("Setting Up Weapons");
        this.ProjectileSpawnPoint = Tank.transform.Find("Turret/Spawn").gameObject;

        this.SideWeaponPrefab = this.MyTankObject.SideWeapon.ammo;

        if (this.MyFireMode == FireMode.Beam)
        {
            this.Beam = PhotonNetwork.Instantiate(MyTankObject.MainWeapon.ammoName, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
            this.Beam.transform.SetParent(ProjectileSpawnPoint.transform);
            this.Beam.SetActive(false);
        }
        else
        {
            this.MainWeaponPrefab = this.MyTankObject.MainWeapon.ammo;
        }
    }

    void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
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
    }

    [PunRPC]
    void UseSideWeapon(string message)
    {
        if (!photonView.isMine)
            return;

        GameObject bulletClone = PhotonNetwork.Instantiate(SideWeaponPrefab.name, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
        //GameObject bulletClone = Instantiate(SideWeaponPrefab, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
        Vector3 direction = ProjectileSpawnPoint.transform.forward;

        bulletClone.GetComponent<WeaponCollider>().team = this.MyTeam;
        bulletClone.GetComponent<WeaponCollider>().Damage = this.MyTankObject.SideWeapon.damage;

        bulletClone.GetComponent<Rigidbody>().AddForce(direction * MyTankObject.SideWeapon.ProjectileSpeed);
        //Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), this.GetComponent<Collider>());
    }

    [PunRPC]
    void UseMainWeapon(string message)
    {
        if (!photonView.isMine)
            return;

        GameObject bulletClone = PhotonNetwork.Instantiate(MainWeaponPrefab.name, ProjectileSpawnPoint.transform.position, Quaternion.identity, 0);
        //GameObject bulletClone = Instantiate(MainWeaponPrefab, ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.rotation);
        Vector3 direction = ProjectileSpawnPoint.transform.forward;

        bulletClone.GetComponent<WeaponCollider>().team = this.MyTeam;
        bulletClone.GetComponent<WeaponCollider>().Damage = this.MyTankObject.MainWeapon.damage;

        bulletClone.GetComponent<Rigidbody>().AddForce(direction * MyTankObject.MainWeapon.ProjectileSpeed);
        //Physics.IgnoreCollision(bulletClone.GetComponent<Collider>(), this.GetComponent<Collider>());
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
            stream.SendNext(this.Health);
            stream.SendNext(this.CDMain);
            stream.SendNext(this.CDSide);
            stream.SendNext(this.MyName);
            stream.SendNext(this.myTankID);
        }
        else
        {
            // Network player, receive data
            this.IsFiringMain = (bool)stream.ReceiveNext();
            this.IsFiringSide = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
            this.CDMain = (float)stream.ReceiveNext();
            this.CDSide = (float)stream.ReceiveNext();
            this.MyName = (string)stream.ReceiveNext();
            this.myTankID = (int)stream.ReceiveNext();
        }
    }

    #endregion
}