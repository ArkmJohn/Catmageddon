using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    STARTING,
    ONGOING,
    OVER
}

public class GameManager : Photon.MonoBehaviour, IPunObservable
{

    static public GameManager Instance;
    public GameObject PlayerPrefab;
    public List<GameObject> SpawnPoints;

    public List<TankObject> TankPrefabs;

    [Tooltip("Add Here the Panel To Show if a player Wins")]
    public GameObject WinPanel;
    [Tooltip("Add Here the Panel To Show if the Player Lost")]
    public GameObject LosePanel;
    [Tooltip("Add Here the Score Panel")]
    public GameObject ScorePanel;

    public int GoalScore;

    public static GameState CurrentGameState = GameState.STARTING;

    [Tooltip("Material for Red Team")]
    public Material RedMaterial;
    [Tooltip("Material for Blue Team")]
    public Material BlueMaterial;
    [Tooltip("Material To Change")]
    public Material TeamMaterial;

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
        Debug.Log(CurrentGameState);
	}

    // Sets up the set tank equipment
    public void SetUp()
    {
        //// Creates the local Player
        if (PhotonNetwork.player.IsLocal)
        {
            GameObject player = PhotonNetwork.Instantiate(this.PlayerPrefab.name, SpawnPoints[PhotonNetwork.player.ID - 1].transform.position, Quaternion.identity, 0); // Spawns the player character based on the Player ID and an array as the reference for the vector

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
        CurrentGameState = GameState.ONGOING;
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

        if (tempBlue >= GoalScore)
        {
            EndState(PunTeams.Team.blue, PunTeams.Team.red);
        }
        if (tempRed >= GoalScore)
        {
            EndState(PunTeams.Team.red, PunTeams.Team.blue);
        }
        RedScoreText.text = tempRed.ToString();
        BlueScoreText.text = tempBlue.ToString();
    }

    public virtual void EndState(PunTeams.Team TeamThatWon, PunTeams.Team TeamThatLost)
    {
        Debug.Log("Game Over");
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("TeamWon", PhotonTargets.All, TeamThatWon);
            photonView.RPC("TeamLose", PhotonTargets.All, TeamThatLost);
            GameManager.CurrentGameState = GameState.OVER;
            //TeamWon(TeamThatWon);
            //TeamLose(TeamThatLost);
        }
    }

    [PunRPC]
    public virtual void TeamWon(PunTeams.Team TeamThatWon)
    {
        this.ScorePanel.SetActive(false);

        if (PhotonNetwork.player.GetTeam() == TeamThatWon)
        {
            this.WinPanel.SetActive(true);
        }
    }

    [PunRPC]
    public virtual void TeamLose(PunTeams.Team TeamThatLost)
    {
        this.ScorePanel.SetActive(false);

        if (PhotonNetwork.player.GetTeam() == TeamThatLost)
        {
            this.LosePanel.SetActive(true);
        }
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

    public void LeaveThisRoom(string GameResult)
    {
        NetworkManager.Instance.LeaveTheRoom(GameResult);
    }
    
    #region IPunObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(GameManager.CurrentGameState);
        }
        else
        {
            GameManager.CurrentGameState = (GameState)stream.ReceiveNext();

        }
    }
    #endregion

}
