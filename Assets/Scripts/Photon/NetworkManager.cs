using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.PunBehaviour
{

    public Text netDebug;
    public string appVersion = "1.0";
    public string pName = "John";
    public string GameScene = "TankScenes";
    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;

    [Tooltip("The current status if the player is connecting or not")]
    public bool isConnecting;

    void Awake()
    {
        // Forces Log Level to be the same as the log level set
        PhotonNetwork.logLevel = Loglevel;
        SetPlayerName(pName);
    }

    // Connects to the lobby if not connected yet and joins a random room
    // TODO: matchmaking
    public void Connect()
    {
        isConnecting = true;

        if (PhotonNetwork.connected)
        {
             PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(appVersion);
        }
    }

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {

            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("We load the GameScene");

        // Load the Room Level. 
        PhotonNetwork.LoadLevel(GameScene);

    }

    public override void OnDisconnectedFromPhoton()
    {

    }
    #endregion

    // Set Name
    public void SetPlayerName(string value)
    {
        PhotonNetwork.playerName = value + " ";// Space so that playername would not be empty
    }
}
