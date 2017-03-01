using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [Tooltip("Particle to be spawned in death")]
    public GameObject destroyParticle;

    [Tooltip("Time til the bullet gets destroyed")]
    [SerializeField]
    float deathTimer = 2f;

    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
            DestroyMe();
    }
    void OnTriggerEnter(Collider col)
    {
        DestroyMe();
    }

    [PunRPC]
    void DestroyMe()
    {
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Destroying in M");
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
            Debug.Log("Not M");

        if (!PhotonNetwork.connected)
            Destroy(this.gameObject);
    }

}
