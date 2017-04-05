using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffectApplication : Photon.PunBehaviour
{

    //Handles PowerUp Effect Distribution
    //Handles duration
    public GameObject Spawner;

    public void DestroyMe()
    {
        Spawner.GetComponent<PowerUpSpawnLocation>().MySpawnedPowerUp = null;
        PhotonView pView = GetComponent<PhotonView>();
        pView.photonView.RPC("DestroyThisPowerup", PhotonTargets.All);
    }

    [PunRPC]
    public virtual void DestroyThisPowerup()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
