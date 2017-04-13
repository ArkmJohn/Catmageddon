using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MouseBehaviour
{
    [Tooltip("Range of this AI when attacking")]
    public float attackRange = 0.2f;
    public bool isAttacking;
    [Tooltip("Attack Collider to be used")]
    public GameObject attackCollider;

    public Animator myAnimator;

    public GameObject AI;
    public GameObject target; //has been changed to dynamic
    public Vector3 seek;
    public Rigidbody RB, targetRB; //has been changed to dynamic rigidbody
    public float Maxspeed;
    public float Maxforce;
    public float distance, avoidanceForce; //change distance to calculate distance between 'this' and dynamic target
    private float coof, raycoof;
    public float Force;
    public bool MOV;
    public Vector3 predictedpos;
    public float frames;
    public float targetmaxspeed; // change to dynamic target's maxspeed
    //private Vector3 ZeroVel;
    public float Multiplier = 1f;
    public float negMultiplier = 1f;
    Vector3 avoidanceDirection, avoidanceDirection2;
    public bool Isfindingnextgoal;

    Vector3 networkAIPos;
    Quaternion networkAIRot;

    #region Unity.Callbacks

    void Start()
    {
        //ZeroVel = new Vector3(0, 0, 0);
        if (myAnimator == null)
        {
            myAnimator = GetComponent<Animator>();
        }
        coof = 1f;
        distance = new float();
        AI = this.gameObject;
        //target = CurrentTarget;
        RB = gameObject.GetComponent<Rigidbody>();
        //AIvelocity = new Vector3();
        //Desiredvelocity = new Vector3();
        seek = new Vector3();
        //targetRB = target.GetComponent<Rigidbody>();

        targetmaxspeed = Maxspeed; // NEEDS TO BE REPLACED WITH THE ACTUAL MAX VELOCITY!!!
        frames = new int();  // this will be the number of frames which is needed to predict HOW much of future
        avoidanceDirection = new Vector3();
        avoidanceDirection2 = new Vector3();
    }

    void Update()
    {
        if (attackCollider.GetComponent<WeaponCollider>().team != this.MyTeam && attackCollider != null)
        {
            attackCollider.GetComponent<WeaponCollider>().team = this.MyTeam;
        }

        if (this.isAttacking != this.attackCollider.GetActive())
        {
            this.attackCollider.SetActive(this.isAttacking);
        }
    }

    void FixedUpdate()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            transform.position = Vector3.Lerp(transform.position, this.networkAIPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.networkAIRot, Time.deltaTime * 5);
        }
        else
        {
            if (this.IsDead())
            {
                // Destroy this
                PhotonNetwork.Destroy(this.gameObject);
            }
            if (target != null)
            {

                float tempDist = DistanceToTarget(target);
                if (tempDist < attackRange)
                {
                    // Attack here
                    myAnimator.SetTrigger("Attack");
                    isAttacking = true;
                    Debug.Log("Attacking");
                    transform.LookAt(target.transform);
                    return;
                }

            }
            if (isAttacking)
                return;

            if (target != null)
            {
                ObstacleAvoidance();
                transform.LookAt(transform.position + RB.velocity);
            }
            if (!Isfindingnextgoal)
            {
                StartCoroutine(FindNextGoalV());
                //FindNextGoal();
            }
            this.photonView.RPC("UpdatePosition", PhotonTargets.Others, this.transform.position, this.transform.rotation);
        }
    }

    [PunRPC]
    void UpdatePosition(Vector3 netPos, Quaternion netQ)
    {
        this.networkAIPos = netPos;
        this.networkAIRot = netQ;
    }

    void OnTriggerStay(Collider other)
    {
        // We dont do anything if we're not the local player
        if (!photonView.isMine)
        {
            return;
        }
        if (other.gameObject.tag != "Weapon")
        {
            return;
        }

        this.Health -= other.GetComponent<WeaponCollider>().Damage * Time.deltaTime;
    }

    #endregion

    #region Public.Function

    public void FinishAttacking()
    {
        this.isAttacking = false;
    }

    #endregion

    #region Private.Functions

    void Seek()
    {


        Vector3 desiredvelocity = target.transform.position - AI.transform.position;
        seek = desiredvelocity - RB.velocity;
        //seek = Vector3.ClampMagnitude(seek, Maxforce);
        RB.AddForce(seek);
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, Maxspeed);

    }

    Rigidbody TargetRB(GameObject obj)
    {
        return obj.GetComponent<Rigidbody>();
    }

    void Distance()
    {
        distance = Vector3.Distance(AI.transform.position, target.transform.position);


        if (distance <= 1000)
        {
            MOV = true;
        }
        else
        {
            MOV = false;
        }
    }

    void FindNextGoal()
    {
        if (target == null)
        {
            switch (Mymousestates)
            {
                case MouseStates.ATTACKING:
                    target = FindTarget();
                    distance = DistanceToTarget(target);
                    targetRB = target.GetComponent<Rigidbody>();
                    break;

                case MouseStates.DEFENDING:
                    target = DefendDominion(target);
                    distance = DistanceToTarget(target);
                    targetRB = target.GetComponent<Rigidbody>();

                    break;

                default:
                    Debug.Log("This is not a valid State");
                    break;
            }


        }
        else if (target != null)
        {
            Distance();



            if (MOV == true)
            {
                Arrive();
                Debug.Log("Arrive");
            }
            else
            {
                predictedVel();
                Debug.Log("Seek");
            }


            //Debug.Log(MOV?"seek":"arrive");
        }
        Isfindingnextgoal = false;
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

    #endregion

    #region IEnumerators

    public IEnumerator FindNextGoalV()
    {
        Isfindingnextgoal = true;
        yield return new WaitForSeconds(0.25f);
        FindNextGoal();
    }

    #endregion

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
            if (hit.collider.gameObject != target)//hit.collider.tag == "Untagged")
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
                if (hit4.collider.gameObject != target)//"Untagged")
                {
                    RB.AddForce(leftVec.normalized * 10f);
                    Debug.Log("move left");
                }
            }
            if (Physics.Raycast(transform.position, leftVec, out hit5, 2 * raycoof))
            {
                if (hit5.collider.gameObject != target)//"Untagged")
                {
                    RB.AddForce(transform.right * 10f);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.Health);
            stream.SendNext(this.isAttacking);

        }
        else
        {
            this.Health = (int)stream.ReceiveNext();
            this.isAttacking = (bool)stream.ReceiveNext();
        }
    }
}