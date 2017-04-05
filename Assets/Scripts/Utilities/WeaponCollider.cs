using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : Photon.PunBehaviour
{

    public float Damage;
    public PunTeams.Team team;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.team);
        }
        else
        {
            this.team = (PunTeams.Team)stream.ReceiveNext();
        }
    }
}
