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
    public string LastWinResult = "None";
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


        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.logLevel = Loglevel;
            PhotonNetwork.automaticallySyncScene = true;
            Connect();
        }
        else
            DebugMe("Connected to Servers");
    }

    public void LeaveTheRoom(string GameResult)
    {
        this.LastWinResult = GameResult;
        PhotonNetwork.LeaveRoom();
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
            PhotonNetwork.player.SetTank(defaultTank);
        }
        if (PhotonNetwork.connected)
        {

            DebugMe("Connecting to Game");
            switch (GameType)
            {
                case "deathmatch":
                    GameScene = "deathmatch";
                    expectedPlayerCount = 2;
                    JoinDeathMatch();
                    break;

                case "koh":
                    GameScene = "koh";
                    //expectedPlayerCount = 4;
                    expectedPlayerCount = 2; // Testing
                    JoinKOH();
                    break;

                case "ctt":
                    GameScene = "ctt";
                    //expectedPlayerCount = 6;
                    expectedPlayerCount = 2; // Testing
                    JoinCTT();
                    break;

                default:
                    Debug.Log("No Game Mode of this type");
                    break;
            }
        }
        else
            DebugMe("ERROR: Not Connected To Server");
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

    public void DebugMe(string Message)
    {
        foreach (ErrorText a in FindObjectsOfType<ErrorText>())
        {
            a.SetWarning(Message);
        }
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            DebugMe("Connected to Servers");
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
                DebugMe("Joining DeathMatch");
                break;

            case "koh":
                PhotonNetwork.CreateRoom(null, KOHProperties(), null);
                //expectedPlayerCount = 4;
                expectedPlayerCount = 2; // Testing

                DebugMe("Joining KoH");
                break;

            case "ctt":
                PhotonNetwork.CreateRoom(null, CTTProperties(), null);
                //expectedPlayerCount = 6;
                expectedPlayerCount = 2; // Testing

                DebugMe("Joining CTT");
                break;

            default:
                DebugMe("Unable To Join/Create Because of an Error.");
                break;
        }

    }

    public override void OnJoinedRoom()
    {
        DebugMe("Waiting for other Players");
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
        //roomOptions.MaxPlayers = 4;
        roomOptions.MaxPlayers = 2;

        return roomOptions;
    }

    public RoomOptions CTTProperties()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "ctt", "ai" };
        roomOptions.CustomRoomProperties = new Hashtable() { { "ctt", 1 } };
        //roomOptions.MaxPlayers = 6;
        roomOptions.MaxPlayers = 2;

        return roomOptions;
    }

    #endregion

    #region Join Room with Optiions
    public void JoinDeathMatch()
    {
        Debug.Log("Joing Room");
        Hashtable expectedRoomProperties = new Hashtable() { { "deathmatch", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 2); // Test
    }

    public void JoinKOH()
    {
        Debug.Log("Joing Room");

        Hashtable expectedRoomProperties = new Hashtable() { { "koh", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 2); // Test
    }

    public void JoinCTT()
    {
        Debug.Log("Joing Room");

        Hashtable expectedRoomProperties = new Hashtable() { { "ctt", 1 } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProperties, 2); // Test
    }
    #endregion

    #region Player Connects/Disconnects Method

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("A Player Joined the Room");
        DebugMe("Waiting for other Players (" + (expectedPlayerCount - PhotonNetwork.room.PlayerCount) + " more player/s needed)");
        // Load the Room Level. 
        if (PhotonNetwork.room.PlayerCount == expectedPlayerCount)
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.room.IsOpen = false; // Closes the Room
                PhotonNetwork.LoadLevel(GameScene);//LoadGame();
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
        if (GameManager.Instance != null && GameManager.CurrentGameState != GameState.OVER)
        {
            // Do Some Stuff if Player Leaves
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Local Player has left the Room");
        PhotonNetwork.LoadLevel("MainMenu");
        if (CurrentPurrofile.Instance != null)
        {
            if (this.LastWinResult == "WIN")
            {
                //CurrentPurrofile.Instance.
                PurrofileHandler.Instance.Win();
            }
            else if (this.LastWinResult == "LOSE")
            {
                PurrofileHandler.Instance.Lose();
            }
            else
            {
                Debug.Log("No Win Result");
            }

        }
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
