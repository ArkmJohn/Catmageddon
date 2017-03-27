using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steerings : MouseBehaviour
{

    public GameObject AI;
    public GameObject target; //has been changed to dynamic
    public Vector3 seek;
    public Rigidbody RB, targetRB; //has been changed to dynamic rigidbody
    public float Maxspeed;
    public float Maxforce;
    public float distance, avoidanceForce; //change distance to calculate distance between 'this' and dynamic target
    private float coof, raycoof;
    public float Force;
    private bool MOV;
    public Vector3 predictedpos;
    public float frames;
    public float targetmaxspeed; // change to dynamic target's maxspeed
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
        target = CurrentTarget;
        RB = gameObject.GetComponent<Rigidbody>();
        //AIvelocity = new Vector3();
        //Desiredvelocity = new Vector3();
        seek = new Vector3();
        targetRB = target.GetComponent<Rigidbody>();

        targetmaxspeed = 10; // NEEDS TO BE REPLACED WITH THE ACTUAL MAX VELOCITY!!!
        frames = new int();  // this will be the number of frames which is needed to predict HOW much of future
        avoidanceDirection = new Vector3();
        avoidanceDirection2 = new Vector3();
    }


    void FixedUpdate()
    {

        //Desire();
        Distance();


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
        ObstacleAvoidance();
        transform.LookAt(transform.position + RB.velocity);

        //Debug.Log(MOV?"seek":"arrive");
    }


    void Seek()
    {


        Vector3 desiredvelocity = target.transform.position - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        //seek = Vector3.ClampMagnitude(seek, Maxforce);
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);
     
    }


    void Distance()
    {
        distance = Vector3.Distance(AI.transform.position, target.transform.position);


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
        Vector3 desiredvelocity = target.transform.position - AI.transform.position;
        RB.AddForce(desiredvelocity - RB.velocity);
        seek = desiredvelocity - RB.velocity;
        coof = distance / 15f - 0.05f;
        coof = Mathf.Clamp(coof, 0, 1f);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed * coof);
 

    }

    void predictedVel()
    {
        frames = distance / targetmaxspeed;
        predictedpos = target.transform.position + targetRB.velocity * frames;
        Vector3 desiredvelocity = predictedpos - AI.transform.position; // Player.transform.position - AI.transform.position; //predictedpos - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);
        transform.LookAt(target.transform.position);
    }

    // -------------------------------------------------------------Sasan's Section Below -----------------------------------------------------------------------------------------------
    // The Obstacle avoidance is Sasan Faizollah's Work mostly , I have only collaborated and assisted with this part and do not claim ownership of the following code.

    void ObstacleAvoidance()
    {
        Vector3 angledVec, neGangledVec, leftVec;
        Vector3 impactPoint, impactPoint2;
        Vector3 objectPos;

        angledVec = Quaternion.AngleAxis((10 * Multiplier), Vector3.up) * transform.forward;
        neGangledVec = Quaternion.AngleAxis((-10 * negMultiplier), Vector3.up) * transform.forward;
        leftVec = Quaternion.AngleAxis(-90f, Vector3.up) * transform.forward;
        Ray myRay = new Ray(transform.position, transform.forward);
        RaycastHit hit, hit2, hit3, hit4, hit5;
        //RaycastHit hit2;
        raycoof = distance / 20f;
        raycoof = Mathf.Clamp01(raycoof);

        if (!(Physics.Raycast(myRay, out hit, 20f * raycoof)))
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
                if (Physics.Raycast(transform.position, angledVec, out hit2, 25f * raycoof))
                {
                    Debug.Log(hit2.collider.name + " is at " + Multiplier * 10f);
                    Multiplier++;
                    impactPoint = hit2.point;
                    objectPos = hit.transform.position;
                    avoidanceDirection = impactPoint - objectPos;
                    avoidanceDirection = avoidanceDirection.normalized * avoidanceForce;
                }
                if (Physics.Raycast(transform.position, neGangledVec, out hit3, 25f * raycoof))
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
                    RB.AddForce(transform.right * 10f);
                    Multiplier = 1f;
                    negMultiplier = 1f;
                }
                else
                {
                    RB.AddForce(avoidanceDirection2);
                    RB.AddForce(transform.right * -10f);
                    Multiplier = 1f;
                    negMultiplier = 1f;
                }

                Debug.Log("Hallo");

            }

            if (Physics.Raycast(transform.position, transform.right, out hit4, 2 * raycoof))
            {
                if (hit4.collider.tag == "Environment")
                {
                    RB.AddForce(leftVec.normalized * 20f);
                    Debug.Log("move left");
                }
            }
            if (Physics.Raycast(transform.position, leftVec, out hit5, 2 * raycoof))
            {
                if (hit5.collider.tag == "Environment")
                {
                    RB.AddForce(transform.right * 20f);
                    Debug.Log("move right");
                }
            }
            //Debug.Log("Hello");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward.normalized * 20f * raycoof);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 2 * raycoof);
        Gizmos.DrawLine(transform.position, transform.position + ((Quaternion.AngleAxis(-90f, Vector3.up) * transform.forward) * 2 * raycoof));
        Gizmos.DrawLine(transform.position, transform.position + ((Quaternion.AngleAxis((10 * Multiplier), Vector3.up) * transform.forward) * 25f * raycoof));
        Gizmos.DrawLine(transform.position, transform.position + ((Quaternion.AngleAxis((-10 * negMultiplier), Vector3.up) * transform.forward) * 25f * raycoof));
    }
}