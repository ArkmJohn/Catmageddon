using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankGameManager : Photon.PunBehaviour
{
    static public TankGameManager Instance;

    // Use this for initialization
    void Start ()
    {
        Instance = this;

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadLevel()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("Trying to Load a level but we are not the master Client");
        }
        PhotonNetwork.LoadLevel("TankScenes");
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
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName);
    }
    #endregion
}
