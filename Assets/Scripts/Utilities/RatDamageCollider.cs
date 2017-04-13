using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatDamageCollider : WeaponCollider
{

    public GameObject myRat;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (team != myRat.GetComponent<CharInfo>().MyTeam)
        {
            team = myRat.GetComponent<CharInfo>().MyTeam;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<CharInfo>())
        {
            if (!PhotonNetwork.connected)
            {
                col.gameObject.GetComponent<CharInfo>().TakeDamage(Damage, team);
            }
            col.gameObject.GetComponent<PhotonView>().photonView.RPC("TakeDamage", PhotonTargets.All, Damage, team);
        }
    }
}
