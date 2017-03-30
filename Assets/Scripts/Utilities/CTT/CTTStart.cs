using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTTStart : MonoBehaviour {

    public int tankID = 0;

    public void StartGame()
    {
        FindObjectOfType<TankGameManager>().StartGame(tankID);
    }
}
