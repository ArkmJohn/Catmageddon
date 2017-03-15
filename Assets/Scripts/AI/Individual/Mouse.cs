using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    public int Health;
    public int Movespeed;
    public int MeleeDamage;
    public float Attackrange;
    public enum MouseStates
    {
        DEFENDING,
        ATTACKING,
        PURSUING,
    }
    MouseStates Mycurrentstate;

    // Use this for initialization
    void Start()
    {

        Health = new int();
        Movespeed = new int();
        MeleeDamage = new int();
        Attackrange = new float();

    }

    // Update is called once per frame
    void Update()
    {


    }


}
