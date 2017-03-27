using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : CharInfo {


    public GameObject OurFlag; // Allied Flag
    public GameObject EnemyFlag; // Enemy team's Flag
    public List<GameObject> MyEnemies = new List<GameObject>(); // List containing all the enemy players
    public List<CharInfo> TotalCharacters = new List<CharInfo>();
    public GameObject CurrentTarget;
    public Rigidbody TargetRB;
    public float currenttargetmaxspeed;


    public enum MouseStates
    {
        ATTACKING,
        DEFENDING
    }

    public MouseStates Mymousestates;


    // Use this for initialization
    void Start()
    {

        CharInfo[] Characters = FindObjectsOfType(typeof(CharInfo)) as CharInfo[];
        foreach (CharInfo Character in Characters)
        {
            if (Character.TeamID != this.TeamID)
            TotalCharacters.Add(Character);
        }
    }
	
	// Update is called once per frame
	void Update () {
		

	}

    public void Attackcheck()
    {
  



    }

    public void Defending()
    {
        if (this.Mymousestates == MouseStates.DEFENDING)
        {
            float Distancetoflag = Vector3.Distance(this.transform.position, OurFlag.transform.position);
            if(Distancetoflag > 50)
            {

                //add steering force towards OURFLAG;
            }

        }
    }

}
