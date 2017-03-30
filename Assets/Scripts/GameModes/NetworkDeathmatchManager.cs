using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkDeathmatchManager : GameManager
{


    void Start()
    {
        SetUp();
    }

    [PunRPC]
    public override void UpdateScore()
    {
        base.UpdateScore();
    }

    public override void PlayerDied(PhotonPlayer player)
    {
        if (player.ID == 0)
        {
            PhotonNetwork.playerList[1].AddScore(1);
        }
        else
        {
            PhotonNetwork.playerList[0].AddScore(1);
        }

        photonView.RPC("UpdateScore", PhotonTargets.All);
    }
}
