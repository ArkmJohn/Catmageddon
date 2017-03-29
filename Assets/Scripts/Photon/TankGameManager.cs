using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankGameManager : Photon.PunBehaviour, IPunObservable
{
    static public TankGameManager Instance;
    public GameObject RedClasses, BlueClasses, PlayersPanel;
    public GameObject PlayerPrefab, RedPortrait, BluePortrait;
    public List<Transform> PlayerSpawnPoints;
    public List<TankObject> TankPrefabs = new List<TankObject>();
    [SerializeField]
    public int PlayersReadied = 0;
    [SerializeField]
    public int maxPlayers = 2;
    // Checks wether time is goin or not
    static public bool GameOnGoing;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        // Initializes Player when entering the scene.
        if (PlayerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (CatInfo.LocalPlayerInstance == null)
            {

                Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (PhotonNetwork.connected)
                {
                    PhotonNetwork.Instantiate(this.PlayerPrefab.name, PlayerSpawnPoints[PhotonNetwork.player.ID - 1].position, Quaternion.identity, 0);
                    this.photonView.RPC("RefreshPlayers", PhotonTargets.All,PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
                }
                else
                {
                    Instantiate(this.PlayerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity);
                    //this.photonView.RPC("RefreshPlayers", PhotonTargets.All, PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
                }
            }
            else
            {
                Debug.Log("Ignoring scene load for " + Application.loadedLevelName);
            }
        }
    }

    public void LoadLevel()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("GameScene");
    }

    [PunRPC]
    public void RefreshPlayers(string Name, int id)
    {
        //Refreshes the list
        foreach(PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (p.GetTeam() == PunTeams.Team.none)
            {
                if (!PlayerTeamIsBlue(p.ID))
                {
                    p.SetTeam(PunTeams.Team.red);
                }
                else
                    p.SetTeam(PunTeams.Team.blue);
            }

            if (p.GetTeam() != PunTeams.Team.none)
            {
                GameObject tempPortrait = PlayersPanel.transform.GetChild(p.ID - 1).gameObject;
                tempPortrait.SetActive(true);
                tempPortrait.transform.GetChild(0).GetComponent<Text>().text = p.NickName;
            }
        }

        // Sets the Panels
        if (!PlayerTeamIsBlue(id))
        {
            RedClasses.SetActive(true);
        }
        else if (PlayerTeamIsBlue(id))
        {
            BlueClasses.SetActive(true);
        }
    }

    public void StartGame(int tankID)
    {
        CatInfo[] cats = FindObjectsOfType<CatInfo>();
        foreach (CatInfo c in cats)
        {
            //c.SetUpTank(tankID); // Since the local player is the one choosing the tank it would be local?
        }

        PlayersReadied++;
        this.photonView.RPC("PlayerReady", PhotonTargets.All,"A Player Readied");
    }

    [PunRPC]
    void PlayerReady(string message)
    {

        Debug.Log(message);
        if (PlayersReadied == maxPlayers)
        {
            GameOnGoing = true;
            Debug.Log("Game Start");
        }
    }

    bool PlayerTeamIsBlue(int id)
    {
        return (id % 2) == 0;
    }

    #region LeaveRoom Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Player Connects/Disconnects Method

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected() " + other.NickName);
        //CreatePlayerEntry(other.NickName, other.ID);
        this.photonView.RPC("RefreshPlayers", PhotonTargets.All, PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
        if (PhotonNetwork.playerList.Length + 1 == maxPlayers)
        {
            PhotonNetwork.room.IsOpen = false; // Closes the room
            Debug.Log("Has Enough Players Closing Room");
        }
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ((IPunObservable)Instance).OnPhotonSerializeView(stream, info);
        if (stream.isWriting)
        {
            foreach (Transform a in PlayersPanel.transform)
            {
                stream.SendNext(a.gameObject.activeSelf);
            }
            stream.SendNext(PlayersReadied);
        }
        else
        {
            //PlayersPanel = (GameObject)stream.ReceiveNext();
            foreach (Transform a in PlayersPanel.transform)
            {
                a.gameObject.SetActive((bool)stream.ReceiveNext());
                //a.transform.GetChild(0).GetComponent<Text>().text = (string)stream.ReceiveNext();
                //stream.SendNext(a.transform.GetChild(0).GetComponent<Text>().text);
            }
            PlayersReadied = (int)stream.ReceiveNext();
        }
    }
}
