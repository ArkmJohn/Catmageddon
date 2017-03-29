using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Photon.MonoBehaviour, IPunObservable
{

    static public GameManager Instance;
    public GameObject PlayerPrefab;
    public List<GameObject> SpawnPoints;

    public List<TankObject> TankPrefabs;
    public int GoalScore;

    #region UIHandlerVariables
    public Text RedScoreText;
    public Text BlueScoreText;
    #endregion

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Sets up the set tank equipment
    public void SetUp()
    {
        //// Creates the local Player
        if (PhotonNetwork.player.IsLocal)
        {
            GameObject player = PhotonNetwork.Instantiate(this.PlayerPrefab.name, SpawnPoints[PhotonNetwork.player.ID - 1].transform.position, SpawnPoints[PhotonNetwork.player.ID - 1].transform.rotation, 0);

            // Sets the Local Player to a Team based on his playerID
            if (PhotonNetwork.player.ID % 2 == 0)
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            }
            else
            {
                PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            }
        }
    }

    // Updates the Score UI on the Canvas
    public virtual void UpdateScore()
    {
        int tempRed = 0;

        int tempBlue = 0;

        foreach (PhotonPlayer a in PhotonNetwork.playerList)
        {
            if (a.GetTeam() == PunTeams.Team.red)
                tempRed += a.GetScore();
            else if (a.GetTeam() == PunTeams.Team.blue)
                tempBlue += a.GetScore();
        }

        RedScoreText.text = tempRed.ToString();
        BlueScoreText.text = tempBlue.ToString();
    }

    public virtual void PlayerDied(PhotonPlayer player)
    {

    }

    public virtual void ReachGoal(PhotonPlayer player)
    {

    }

    public virtual void OnTheHill(PhotonPlayer player)
    {

    }

    public virtual void GameOver()
    {

    }

    #region IPunObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {


        }
    }
    #endregion

}
