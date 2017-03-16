using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInfo : Photon.PunBehaviour, IPunObservable {

    [Tooltip("Can be filled by the Tank ID in the game manager. It starts from 0. Provided there is already a set Game Manager and tanks for it to hold")]
    public int myDefaultTankID;

    public float maxHealth;
    [Tooltip("Cooldown for the amount of time given for the player before it is damage next by a weapon")]
    public float damageCooldown = 0.5f;
    [Tooltip("The current Health of our player")]
    public float health;
    public static GameObject LocalCatInstance;
    float damageCD;
    bool gotDamaged = false;
    TankObject myTankBP;

    #region Unity Callbacks

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (PhotonNetwork.connected)
        {
            if(photonView.isMine)
                CatInfo.LocalCatInstance = this.gameObject;
        }
        else
            CatInfo.LocalCatInstance = this.gameObject;
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        //SetUpTank(myDefaultTankID);
    }

    // Update is called once per frame
    void Update()
    {
        if (gotDamaged)
        {

            if (damageCD <= 0)
            {
                gotDamaged = false;
                damageCD = damageCooldown;
            }
            else
            {
                damageCD -= Time.deltaTime;
            }
        }

        if (PhotonNetwork.connected && photonView.isMine)
        {
            Debug.Log("Player ID is " + PhotonNetwork.player.ID);
        }
    }
    #endregion

    #region Public Methods
    [ContextMenu("Reset Tank")]
    public void ResetTank()
    {
        
        SetUpTank(myDefaultTankID);
    }

    [PunRPC]
    public void SetUpTank(int tankIndex)
    {
        if (!PhotonNetwork.connected)
        {
            TankObject myTank = FindObjectOfType<TankGameManager>().TankPrefabs[tankIndex];
            GameObject tank = Instantiate(myTank.TankPrefab, this.transform.position, Quaternion.identity);
            tank.transform.SetParent(this.transform);

            maxHealth = myTank.Health;
            health = maxHealth;
            damageCD = damageCooldown;
            GetComponent<CatWeapons>().SetUpWeapons(tank, myTank);
            myTankBP = myTank;
            GetComponent<CatMovement>().speed = myTankBP.Speed;
            SpawnCatModel(tank.transform.Find("Turret/CatSpawn").gameObject, tank.transform.Find("Turret").gameObject);
        }
        else if (photonView.isMine && PhotonNetwork.connected)
        {
            Debug.Log(tankIndex);
            TankObject myTank = FindObjectOfType<TankGameManager>().TankPrefabs[tankIndex];
            myTankBP = myTank;
            GameObject tank = PhotonNetwork.Instantiate(myTank.TankPrefab.name, this.transform.position, Quaternion.identity, 0);
            tank.transform.SetParent(this.transform);
            SpawnCatModel(tank.transform.Find("Turret/CatSpawn").gameObject, tank.transform.Find("Turret").gameObject);
            maxHealth = myTank.Health;
            health = maxHealth;
            damageCD = damageCooldown;
            GetComponent<CatWeapons>().SetUpWeapons(tank, myTank);
            
            GetComponent<CatMovement>().speed = myTankBP.Speed;
        }

    }

    public void GetDamaged(float damage)
    {
        if (!gotDamaged)
        {
            Debug.Log("got Damaged");
            gotDamaged = true;

            health -= damage;
            if (health <= 0)
            {
                Dead();
            }
        }
    }
    #endregion

    #region Private Methods
    [PunRPC]
    void SpawnCatModel(GameObject catSpawnPoint, GameObject parent)
    {
        // TODO: Spawn based on the profile
        GameObject cat;
        if (!PhotonNetwork.connected)
        {
            cat = Instantiate(myTankBP.CatPrefab, catSpawnPoint.transform.position, Quaternion.identity);
        }
        else if (photonView.isMine && PhotonNetwork.connected)
        {
            cat = PhotonNetwork.Instantiate(myTankBP.CatPrefab.name, catSpawnPoint.transform.position, Quaternion.identity, 0);
            cat.transform.SetParent(parent.transform);
            cat.transform.localScale = Vector3.one * 100;
            cat.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        }
        
    }

    // TODO: RPC Please
    void Dead()
    {
        Debug.Log("An Idiot Died");
    }
    #endregion

    #region Collision Methods
    void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (other.gameObject.layer != 8)
        {
            return;
        }
		if (other.gameObject.tag == "PowerUp") 
		{
			other.gameObject.GetComponent<PowerUpEffectApplication> ().ApplyPowerUpEffect (PhotonNetwork.player.ID);
		}
		//Pickup interaction for the trash cans
		if (other.gameObject.tag == "TrashCan") 
		{
			if (other.gameObject.GetComponent<TrashCanValues> ().GetPickUpState () == false)
			{
//				string teamIdentifier = other.gameObject.GetComponent<TrashCanValues> ().teamID;
//				if (teamIdentifier == "blueTeam" && Player is red) 
//				{
//					GameObject.FindGameObjectWithTag ("TrashCanManager").GetComponent<CaptureTheTrashManager> ().PickUpTrashCan (this.transform, teamIdentifier);
//				}
//				else if (teamIdentifier == "redTeam" && Player is blue) 
//				{
//					GameObject.FindGameObjectWithTag ("TrashCanManager").GetComponent<CaptureTheTrashManager> ().PickUpTrashCan (this.transform, teamIdentifier);
//				}
			}
		}

        Debug.Log("Warning: should Apply Damage");
        //GetDamaged(other.GetComponent<ProjectileController>().damage);

    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.isMine)
        {
            return;
        }
        if (other.gameObject.layer != 8 )
        {
            return;
        }

        Debug.Log("Warning: should Apply Damage");

        // GetDamaged(other.GetComponent<ProjectileController>().damage);
    }

    #endregion

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new NotImplementedException();
    }

}
