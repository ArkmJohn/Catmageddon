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
    public List<TankObject> TankPrefabs = new List<TankObject>();

    // Checks wether time is goin or not
    static public bool GameOnGoing;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        // Initializes Player when entering the scene.
        if (PlayerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (CatInfo.LocalCatInstance == null)
            {

                Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                if (PhotonNetwork.connected)
                {
                    PhotonNetwork.Instantiate(this.PlayerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                    CreatePlayerEntry(PhotonNetwork.player.NickName, PhotonNetwork.player.ID);
                }
                else
                {
                    Instantiate(this.PlayerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity);
                    CreatePlayerEntry("John", 1);
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
    public void CreatePlayerEntry(string Name, int id)
    {
        // Sets the Red Players
        if (!PlayerTeamIsBlue(id))
        {
            GameObject portrait = PlayersPanel.transform.GetChild(id - 1).gameObject;
            portrait.SetActive(true);
            portrait.transform.GetChild(0).GetComponent<Text>().text = Name;
            RedClasses.SetActive(true);
        }
        else if (PlayerTeamIsBlue(id))
        {
            GameObject portrait = PlayersPanel.transform.GetChild(id - 1).gameObject;
            portrait.SetActive(true);
            portrait.transform.GetChild(0).GetComponent<Text>().text = Name;
            BlueClasses.SetActive(true);
        }
    }

    public static void StartGame(int tankID)
    {
        if (PhotonNetwork.connected)
        {
            // Sets up the tank
            CatInfo[] cats = FindObjectsOfType<CatInfo>();
            foreach (CatInfo c in cats)
            {
                c.SetUpTank(tankID); // Since the local player is the one choosing the tank it would be local?
            }
        }
        else
            FindObjectOfType<CatInfo>().SetUpTank(tankID);
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
        CreatePlayerEntry(other.NickName, other.ID);
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
        }
    }
}
