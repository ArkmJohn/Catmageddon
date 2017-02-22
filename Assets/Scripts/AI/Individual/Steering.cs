using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {
    public GameObject AI;
    public GameObject Player;
    public Vector3 seek;
    public Rigidbody RB;
    public Rigidbody PlayerRB;
    public float Maxspeed;
    public float Maxforce;
    public float distance;
    private float coof;
    public float Force;
    private bool MOV;
    public Vector3 predictedpos;
    public float frames;
    public float playermaxspeed;

    void Start()
    {
        coof = 1f;
        distance = new float();
        AI = this.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        RB = gameObject.GetComponent<Rigidbody>();
        PlayerRB = Player.GetComponent<Rigidbody>();
        seek = new Vector3();

        playermaxspeed = 10; // NEEDS TO BE REPLACED WITH THE ACTUAL MAX VELOCITY!!!
        frames = new int();  // this will be the number of frames which is needed to predict HOW much of future
    }


    void FixedUpdate()
    {
        Distance();

        frames = distance / playermaxspeed;
        predictedpos = Player.transform.position + PlayerRB.velocity * frames;

        if (MOV == true)
        {
            Arrive();
            Debug.Log("Arrive");
        }
        else
        {
            Seek();
            Debug.Log("Seek");
        }

    }

    void Seek()
    {
        Vector3 desiredvelocity = predictedpos - AI.transform.position; // Player.transform.position - AI.transform.position; //predictedpos - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);
    }


    void Distance()
    {
        distance = Vector3.Distance(AI.transform.position, Player.transform.position);


        if (distance <= 10)
        {
            MOV = true;
        }
        else
        {
            MOV = false;
        }
    }


    void Arrive()
    {
        Vector3 desiredvelocity = predictedpos - AI.transform.position;// Player.transform.position - AI.transform.position; //predictedpos - AI.transform.position;
        RB.AddForce(desiredvelocity - RB.velocity);

        coof = distance / 30f - 0.05f;
        coof = Mathf.Clamp(coof, 0, 1f);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed * coof);

    }
}
