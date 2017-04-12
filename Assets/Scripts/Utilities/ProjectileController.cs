using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : WeaponCollider {

    [Tooltip("Particle to be spawned in death")]
    public GameObject destroyParticle;
    public int teamID;

    [Tooltip("Time til the bullet gets destroyed")]
    [SerializeField]
    float deathTimer = 2f;

    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            PhotonView pView = GetComponent<PhotonView>();

            //pView.photonView.RPC("DestroyMe", PhotonTargets.All);
            DestroyMe();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<CharInfo>())
        {
            col.gameObject.GetComponent<PhotonView>().photonView.RPC("TakeDamage", PhotonTargets.All, Damage, team);
            DestroyTheObject();
        }
        else
            DestroyMe();
    }

    public void DestroyTheObject()
    {
        PhotonView pView = GetComponent<PhotonView>();
        pView.photonView.RPC("DestroyMe", PhotonTargets.All);

    }

    [PunRPC]
    void DestroyMe()
    {
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        //if (PhotonNetwork.isMasterClient)
        //{
        //    Debug.Log("Destroying in M");
        //    PhotonNetwork.Destroy(this.gameObject);
        //}
        //else
        //    Debug.Log("Not M");

        //if (!PhotonNetwork.connected)
            Destroy(this.gameObject);
    }

}
