using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steerings : MonoBehaviour
{

    public GameObject AI;  
    public GameObject Player;
    public Vector3 seek;
    public Rigidbody RB, playerRB;
    public float Maxspeed;
    public float Maxforce;
    public float distance, avoidanceForce;
    private float coof, raycoof;
    public float Force;
    private bool MOV;
    public Vector3 predictedpos;
    public float frames;
    public float playermaxspeed;
    //private Vector3 ZeroVel;
    public float Multiplier = 1f;
    public float negMultiplier = 1f;
    Vector3 avoidanceDirection, avoidanceDirection2;

    void Start()
    {
        //ZeroVel = new Vector3(0, 0, 0);

        coof = 1f;
        distance = new float();
        AI = this.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        RB = gameObject.GetComponent<Rigidbody>();
        //AIvelocity = new Vector3();
        //Desiredvelocity = new Vector3();
        seek = new Vector3();
        playerRB = Player.GetComponent<Rigidbody>();

        playermaxspeed = 10; // NEEDS TO BE REPLACED WITH THE ACTUAL MAX VELOCITY!!!
        frames = new int();  // this will be the number of frames which is needed to predict HOW much of future
        avoidanceDirection = new Vector3();
        avoidanceDirection2 = new Vector3();
    }


    void FixedUpdate()
    {

        //Desire();
        Distance();
        ObstacleAvoidance();

        //    /*
        if (MOV == true)
        {
            Arrive();
            Debug.Log("Arrive");
        }
        else
        {
            //Seek();
            predictedVel();
            Debug.Log("Seek");
        }
        //    */
        //Debug.Log(MOV?"seek":"arrive");
    }



    //void Desire()
    //{
    //    float distance = new float();
    //    float scale = new float();
    //    distance = Mathf.Sqrt(((Player.transform.position.x - AI.transform.position.x)* (Player.transform.position.x - AI.transform.position.x))
    //        + ((Player.transform.position.y - AI.transform.position.y) * (Player.transform.position.y - AI.transform.position.y))
    //        + ((Player.transform.position.z - AI.transform.position.z) * (Player.transform.position.z - AI.transform.position.z)));
    //    scale = distance / 10;
    //    Desiredvelocity = Player.transform.position - AI.transform.position;
    //    Desiredvelocity = Vector3.ClampMagnitude(Desiredvelocity, Maxspeed);
    //    if(distance <= 10)
    //    {
    //        Desiredvelocity = Vector3.ClampMagnitude(Desiredvelocity, (Maxspeed * scale));
    //    }
    //}

    void Seek()
    {


        Vector3 desiredvelocity = Player.transform.position - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        //seek = Vector3.ClampMagnitude(seek, Maxforce);
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);
        transform.LookAt(Player.transform.position);
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
        Vector3 desiredvelocity = Player.transform.position - AI.transform.position;
        RB.AddForce(desiredvelocity - RB.velocity);
        seek = desiredvelocity - RB.velocity;
        coof = distance / 15f - 0.05f;
        coof = Mathf.Clamp(coof, 0, 1f);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed * coof);
        transform.LookAt(Player.transform.position);



        //RB.velocity = Vector3.ClampMagnitude(desiredvelocity, Maxspeed * coof);
        /*     if (distance <= 10)
             {

                 coof = distance / 10f;

             }
             //Seek();
         */

    }

    void predictedVel()
    {
        frames = distance / playermaxspeed;
        predictedpos = Player.transform.position + playerRB.velocity * frames;
        Vector3 desiredvelocity = predictedpos - AI.transform.position; // Player.transform.position - AI.transform.position; //predictedpos - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);
        transform.LookAt(Player.transform.position);
    }

    void ObstacleAvoidance()
    {
        Vector3 angledVec, neGangledVec;
        Vector3 impactPoint, impactPoint2;
        Vector3 objectPos;

        angledVec = Quaternion.AngleAxis((10 * Multiplier), Vector3.up) * transform.forward;
        neGangledVec = Quaternion.AngleAxis((-10 * negMultiplier), Vector3.up) * transform.forward;
        Ray myRay = new Ray(transform.position, transform.forward);
        RaycastHit hit, hit2, hit3;
        //RaycastHit hit2;
        raycoof = distance / 20f;
        raycoof = Mathf.Clamp01(raycoof);

        if (!(Physics.Raycast(myRay, out hit, 20f)))
        {
            Multiplier = 1f;
            negMultiplier = 1f;
            // avoidanceDirection = null;
        }

        if (Physics.Raycast(myRay, out hit, 20f * raycoof))
        {
            if (hit.collider.tag == "Environment")
            {
                Debug.Log(hit.collider.name + " is ahead");
                if (Physics.Raycast(transform.position, angledVec, out hit2, 30f))
                {
                    Debug.Log(hit2.collider.name + " is at " + Multiplier * 10f);
                    Multiplier++;
                    impactPoint = hit2.point;
                    objectPos = hit.transform.position;
                    avoidanceDirection = impactPoint - objectPos;
                    avoidanceDirection = avoidanceDirection.normalized * avoidanceForce;
                }
                if (Physics.Raycast(transform.position, neGangledVec, out hit3, 30f))
                {
                    Debug.Log(hit3.collider.name + " is at " + negMultiplier * -10f);
                    negMultiplier++;
                    impactPoint2 = hit3.point;
                    objectPos = hit.transform.position;
                    avoidanceDirection2 = impactPoint2 - objectPos;
                    avoidanceDirection2 = avoidanceDirection2.normalized * avoidanceForce;
                }

                //Debug.Log(hit2.collider.name);
                if (Multiplier < negMultiplier)
                {
                    RB.AddForce(avoidanceDirection);
                    Multiplier = 1f;
                    negMultiplier = 1f;
                }
                else
                {
                    RB.AddForce(avoidanceDirection2);
                    Multiplier = 1f;
                    negMultiplier = 1f;
                }

                Debug.Log("Hallo");

            }
            //Debug.Log("Hello");
        }
    }

    private void LateUpdate()
    {
        // transform.LookAt(Player.transform.position);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, transform.forward.normalized * 20f);
    }
}