using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkKingOfTheTrash : GameManager {

    public List<GameObject> Flags;

    void Start()
    {
        SetUp();
    }

    void Update()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }
        if (!IsProcessing)
        {
            StartCoroutine(Process());
        }
    }

    [PunRPC]
    // Updates the Score UI on the Canvas
    public override void UpdateScore()
    {
        base.UpdateScore();
    }

    bool IsProcessing = false;

    public IEnumerator Process()
    {
        IsProcessing = true;
        yield return new WaitForSeconds(0.5f);
        photonView.RPC("ProcessFlags", PhotonTargets.All);
        IsProcessing = false;
    }

    [PunRPC]
    public void ProcessFlags()
    {
        if (PhotonNetwork.player.IsMasterClient)
        {
            foreach (GameObject flag in Flags)
            {
                PunTeams.Team flagTeam = flag.GetComponent<Flag>().FlagOwner;

                switch (flagTeam)
                {
                    case PunTeams.Team.red:
                        PhotonNetwork.playerList[1].AddScore(1);
                        break;

                    case PunTeams.Team.blue:
                        PhotonNetwork.playerList[0].AddScore(1);
                        break;

                    default:

                        break;
                }
            }
        }
        photonView.RPC("UpdateScore", PhotonTargets.All);
    }

    public override void PlayerDied(PhotonPlayer player)
    {
        if (player.GetTeam() == PunTeams.Team.red)
        {
            PhotonNetwork.player.AddScore(1);
        }
        else
        {
            PhotonNetwork.player.AddScore(1);
        }

        photonView.RPC("UpdateScore", PhotonTargets.All);
    }
}
