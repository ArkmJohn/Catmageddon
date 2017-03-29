using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : Photon.PunBehaviour, IPunObservable
{
    public static NetworkManager instance = new NetworkManager();
    public GameObject PlayerPrefab, SpawnObject;
    public Text netDebug;
    public string appVersion = "1.0";
    public string GameScene = "TankScenes";
    public string nick;
    public int defaultTank = 1;
    public int expectedPlayerCount = 2;
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    [Tooltip("The current status if the player is connecting or not")]
    public bool isConnecting;

    #region Instance and things

    static NetworkManager()
    {
    }

    private NetworkManager()
    {
    }

    public static NetworkManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Forces Log Level to be the same as the log level set
        PhotonNetwork.logLevel = Loglevel;
        PhotonNetwork.automaticallySyncScene = true;
        Connect();
    }

    // Connects to the lobby if not connected yet and joins a random room
    public void Connect()
    {
        isConnecting = true;
        if (FindObjectOfType<CurrentPurrofile>())
        {
            PhotonNetwork.player.NickName = CurrentPurrofile.Instance.username + " ";
            PhotonNetwork.player.SetTank(CurrentPurrofile.Instance.equippedTank);
        }
        else
        {
            PhotonNetwork.player.NickName = "Test";
            PhotonNetwork.player.SetTank(defaultTank);
        }
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(appVersion);
        }
        nick = PhotonNetwork.player.NickName;
    }

    // Use 'deathmatch', 'koh', and 'ctt'
    public void ConnectToRoom(string GameType)
    {
        Debug.Log("Hello Got Clicked");
        if (FindObjectOfType<CurrentPurrofile>())
        {
            PhotonNetwork.player.SetTank(CurrentPurrofile.Instance.equippedTank);
        }
        else
        {
            PhotonNetwork.player.NickName = "Test";
            PhotonNetwork.player.SetTank(2);
        }
        if (PhotonNetwork.connected)
        {

            netDebug.text = "Connecting to Game";
            switch (GameType)
            {
                case "deathmatch":
                    GameScene = "deathmatch";
                    expectedPlayerCount = 2;
                    JoinDeathMatch();
                    break;

                case "koh":
                    GameScene = "koh";
                    expectedPlayerCount = 4;
                    JoinKOH();
                    break;

                case "ctt":
                    GameScene = "ctt";
                    expectedPlayerCount = 6;
                    JoinCTT();
                    break;

                default:
                    Debug.Log("No Game Mode of this type");
                    break;
            }
        }
        else
            netDebug.text = "ERROR: Not Connected To Server";
    }

    void LoadGame()
    {
        this.photonView.RPC("LoadGameScene", PhotonTargets.All, GameScene);
    }

    [PunRPC]
    public void LoadGameScene(string message)
    {
        netDebug.text = "Loading Game " + message;
        PhotonNetwork.LoadLevel(GameScene);
    }

    public IEnumerator StartTheGame()
    {
        yield return new WaitForSeconds(5);
        LoadGameScene("Loading Game");
    }

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            netDebug.text = "Connected to Servers";
            //PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Creating a room");
        // Creates Rooms with Custom Properties
        switch (GameScene)
        {
            case "deathmatch":
                PhotonNetwork.CreateRoom(null, DeathMatchProperties(), null);
                expectedPlayerCount = 2;
                netDebug.text = "Joining DeathMatch";
                break;

            case "koh":
                PhotonNetwork.CreateRoom(null, KOHProperties(), null);
                expectedPlayerCount = 4;
                netDebug.text = "Joining KoH";
                break;

            case "ctt":
                PhotonNetwork.CreateRoom(null, CTTProperties(), null);
                expectedPlayerCount = 6;
                netDebug.text = "Joining CTT";
                break;

            default:
                netDebug.text = "Unable To Join/Create Because of an Error.";
                break;
        }

    }

    public override void OnJoinedRoom()
    {
        netDebug.text = "Waiting for other Players";
        // Creates the local Player
    }

    public override void OnDisconnectedFromPhoton()
    {

    }
    #endregion

    #region Custom Room Options
    public RoomOptions DeathMatchProperties()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "deathmatch", "ai" };
        roomOptions.CustomRoomProperties = new Hashtable() { { "deathmatch", 1} };
        roomOptions.MaxPlayers = 2;

        return roomOptions;
    }

    public RoomOptions KOHProperties()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "koh", "ai" };
        roomOptions.CustomRoomProperties = new Hashtable() { { "koh", 1 } };
        roomOptions.MaxPlayers = 4;

        return roomOptions;
    }

    public RoomOptions CTTProperties()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "ctt", "ai" };
        roomOptions.CustomRoomProperties = new Hashtable() { { "ctt", 1 } };
        roomOptions.MaxPlayers = 6;

        return roomOptions;
    }

    #endregion

    #region Join Room with Optiions
    public void JoinDeathMatch()
    {
        Debug.Log("Joing Room");
        Hashtable expectedRoomProperties = new Hashtable() { { "deathmatch", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 2);
    }

    public void JoinKOH()
    {
        Debug.Log("Joing Room");

        Hashtable expectedRoomProperties = new Hashtable() { { "koh", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 4);
    }

    public void JoinCTT()
    {
        Debug.Log("Joing Room");

        Hashtable expectedRoomProperties = new Hashtable() { { "ctt", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 6);
    }
    #endregion

    #region Player Connects/Disconnects Method

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("A Player Joined the Room");
        netDebug.text = "Waiting for other Players (" + (expectedPlayerCount - PhotonNetwork.room.PlayerCount) + " more player/s needed)";
        // Load the Room Level. 
        if (PhotonNetwork.room.PlayerCount == expectedPlayerCount)
        {
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.LoadLevel(GameScene);//LoadGame();

        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
