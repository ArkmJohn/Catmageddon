using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : CharInfo, IPunObservable
{
    public GameObject attackCollider;
    public GameObject target;
    public Vector3 steering;
    public bool isAttacking;
    public float maxSpeed = 5;
    public float slowRadius = 8;
    public float attackRadius = 3;
    public float maxSeeAhead = 16;
    public float maxAvoidForce = 20;
    Rigidbody myRB;
    Animator myAnim;

    // Use this for initialization
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        this.target = FindObjectTarget();
        this.myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Controls the attack Collider
        if (this.attackCollider.GetActive() != this.isAttacking)
        {
            this.attackCollider.SetActive(this.isAttacking);
        }
    }

    void FixedUpdate()
    {
        //if (photonView.isMine)
        //{
        if (PhotonNetwork.player.IsLocal && this.GetComponent<CharInfo>().Health > 0 && GameManager.CurrentGameState == GameState.ONGOING)
        {
            if (!isFindingTarget)
            {
                StartCoroutine(FindTarget());
            }
            if (this.target != null)
            {
                ChaseTarget();
            }
            this.myRB.velocity = steering;
        }
        //}
        //else
        //{
            
        //}
    }

    void ChaseTarget()
    {
        Vector3 desiredVelocity = target.transform.position - this.transform.position;

        float distanceToTarget = desiredVelocity.magnitude;

        //Debug.Log(distanceToTarget);
        if(this.isAvoiding)
        {
            steering = AvoidanceForce();
            steering.Normalize();
            Debug.Log("Hello?");
            this.isAvoiding = false;
        }
        else if (distanceToTarget < slowRadius && distanceToTarget < attackRadius)
        {
            Debug.Log("Attack Here");
            myAnim.SetTrigger("Attack");
            isAttacking = true;
            steering = Vector3.zero;
        }
        else if (distanceToTarget < slowRadius && distanceToTarget > attackRadius && !isAttacking)
        {
            desiredVelocity = Vector3.Normalize(desiredVelocity) * maxSpeed * (distanceToTarget / slowRadius);
            Debug.Log("Slowing Down");
            steering = desiredVelocity - myRB.velocity;
            steering += AvoidanceForce();
            steering.Normalize();

        }
        else
        {
            desiredVelocity = Vector3.Normalize(desiredVelocity) * maxSpeed;
            steering = desiredVelocity - myRB.velocity;
            steering += AvoidanceForce();
            steering.Normalize();

        }

        this.transform.LookAt(steering + transform.position);
    }

    bool isFindingTarget = false;
    IEnumerator FindTarget()
    {
        isFindingTarget = true;
        yield return new WaitForSeconds(2.0f);
        this.target = FindObjectTarget();
        isFindingTarget = false;
    }

    protected List<CharInfo> GetEnemies()
    {
        CharInfo[] Characters = FindObjectsOfType<CharInfo>();
        List<CharInfo> Enemy = new List<CharInfo>();
        foreach (CharInfo c in Characters)
        {
            if (c.MyTeam != this.MyTeam)
            {
                Enemy.Add(c);
            }
        }

        return Enemy;
    }

    protected GameObject FindObjectTarget()
    {
        List<CharInfo> CharacterList = GetEnemies();
        Debug.Log(CharacterList.Count);

        float tempDist = 999999;
        CharInfo target = new CharInfo();
        foreach (CharInfo E in CharacterList)
        {
            float dist = Vector3.Distance(this.transform.position, E.transform.position);
            //Debug.Log(dist);
            if (dist <= tempDist)
            {
                Debug.Log("Setting Up");
                target = E;
                tempDist = dist;
            }
        }

        return target.gameObject;
    }

    bool isAvoiding = false;

    protected Vector3 AvoidanceForce()
    {
        Vector3 avoidanceForce = Vector3.zero;

        Vector3 head = new Vector3(this.transform.position.x, 1, this.transform.position.z);
        Vector3 ahead = head + this.transform.forward * maxSeeAhead;
        Vector3 leftAhead = Quaternion.AngleAxis(-15, this.transform.up) * this.transform.forward;
        Vector3 rightAhead = Quaternion.AngleAxis(15, this.transform.up) * this.transform.forward;

        Ray centerRay = new Ray(head, ahead);
        Ray leftRay = new Ray(head, leftAhead);
        Ray rightRay = new Ray(head, rightAhead);

        RaycastHit hit;

        if (Physics.Raycast(centerRay, out hit, maxSeeAhead))
        {
            if (hit.collider.gameObject != target)
            {
                //avoidanceForce = ahead - hit.collider.gameObject.transform.position;
                //avoidanceForce = Vector3.Normalize(avoidanceForce) * maxAvoidForce;
                //avoidanceForce.y = 0;
                Vector3 impactPoint = hit.point;
                Vector3 avoidanceDir = impactPoint - hit.collider.gameObject.transform.position;
                Debug.DrawLine(impactPoint, avoidanceDir + transform.position);
                avoidanceForce = impactPoint + ((avoidanceDir + transform.position).normalized * maxAvoidForce);

                isAvoiding = true;
                //avoidanceForce = avoidanceDir.normalized * maxAvoidForce;
                Debug.Log("Adding avoidance" + avoidanceForce);
                return avoidanceForce;
            }
        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, slowRadius);
        Vector3 head = new Vector3(this.transform.position.x, 1, this.transform.position.z);
        Vector3 ahead = head + this.transform.forward * maxSeeAhead;
        Gizmos.DrawLine(head, ahead);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawWireSphere(steering.normalized + transform.position, 1);

    }

    public void FinishAttacking()
    {
        this.isAttacking = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.Health);
            stream.SendNext(this.isAttacking);
            //stream.SendNext(this.transform.position);
            //stream.SendNext(this.transform.rotation);
        }
        else
        {
            this.Health = (int)stream.ReceiveNext();
            this.isAttacking = (bool)stream.ReceiveNext();
            //this.transform.position = (Vector3)stream.ReceiveNext();
            //this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
