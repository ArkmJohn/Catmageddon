using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : CharInfo
{


    public GameObject OurFlag; // Allied Flag
    public GameObject EnemyFlag; // Enemy team's Flag
    public float currenttargetmaxspeed; // this AI's current target's maxspeed


    public enum MouseStates // this are the individual states of the AI
    {
        ATTACKING,
        DEFENDING
    }

    public MouseStates Mymousestates; // Mymousestates to be able to SET 'this' AI's state.
    public bool Attacking()
    {
        return this.Mymousestates.Equals(MouseStates.ATTACKING);
    }
    public bool Defending()
    {
        return this.Mymousestates.Equals(MouseStates.DEFENDING);
    }

    bool IsChecking = false; //this bool is a check for the TotalCharacterCheck function , used as a condition to run the function
    bool IsAttacking = false; //this bool is a check for the Attacking coroutine , it is used as a condition to run the function
    bool IsAdding = false; //this bool is a check for the ChartoEnemy function , it is used as a condition to run the function
                           // Update is called once per frame
                           /* void Update()
                            {
                                if (!IsChecking) //running the coroutine to keep updating the total number of characters in the game scene
                                {
                                    StartCoroutine(TotalCharacterCheck());
                                }
                                if (!IsAdding) //running the coroutine to keep updating the enemies of the gameobject
                                {
                                    StartCoroutine(ChartoEnemy());

                                }
                                if (!IsAttacking && Attacking()) //checking if the state of the AI is attacking , if true then starting the attackcheck coroutine
                                {
                                    StartCoroutine(AttackCheck());
                                }

                            } */


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

    protected GameObject FindTarget()
    {
        List<CharInfo> CharacterList = GetEnemies();
        Debug.Log(CharacterList.Count);

        float tempDist = 999999;
        CharInfo target = new CharInfo();
        foreach (CharInfo E in CharacterList)
        {
            float dist = Vector3.Distance(this.transform.position, E.transform.position);
            Debug.Log(dist);
            if (dist <= tempDist)
            {
                Debug.Log("Setting Up");
                target = E;
                tempDist = dist;
            }
        }

        return target.gameObject;
    }

    protected GameObject FindTarget(GameObject FromObject)
    {
        List<CharInfo> CharacterList = GetEnemies();

        float tempDist = 999999;
        CharInfo target = new CharInfo();
        foreach (CharInfo E in CharacterList)
        {
            float dist = Vector3.Distance(FromObject.transform.position, E.transform.position);

            if (dist <= tempDist)
            {
                target = E;
                tempDist = dist;
            }
        }

        return target.gameObject;
    }

    protected float DistanceToTarget(GameObject TargetObj)
    {
        return Vector3.Distance(this.transform.position, TargetObj.transform.position);
    }

    public GameObject DefendDominion(GameObject TargetFlag)
    {
        Flag[] Flags = FindObjectsOfType<Flag>();
        List<Flag> MyFlags = new List<Flag>();
        float tempdist = 999999;
        Flag dtarget = new Flag();
        foreach (Flag f in Flags)
        {
            if (f.FlagOwner == MyTeam)
            {
                MyFlags.Add(f);
            }
        }
                foreach (Flag a in MyFlags)
                {
            float dist = DistanceToTarget(a.gameObject);
            if (dist <= tempdist && a.IsBeingCaptured)
            {
                dtarget = a;
                tempdist = dist;
                this.Mymousestates = MouseStates.DEFENDING;
            }
        }
        return dtarget.gameObject;
    } //for john to do

        /*  public void DefendCTT()
          {

          }

      */ // for john to do

        /*
        #region OldStuff
        public IEnumerator AttackCheck()
        {
            Debug.Log("getting the closest enemy");
            IsAttacking = true;
            yield return new WaitForSeconds(2.0f);
            AttackcheckV();
        }

        public void AttackcheckV()
        {
            float tempDist = 999999;
            CharInfo target = new CharInfo();
            foreach (CharInfo Enemy in MyEnemies)
            {
                float distance = Vector3.Distance(this.transform.position, Enemy.transform.position);
                //Distances.Add(distance);
                //Distances.Sort();
                if (distance <= tempDist)
                {
                    target = Enemy;
                    tempDist = distance;
                }   
            }
            if (target != null)
                CurrentTarget = target.gameObject;
            IsAttacking = false;

        }

        public void Defendcheck()
        {
            if (this.Mymousestates == MouseStates.DEFENDING)
            {
                float Distancetoflag = Vector3.Distance(this.transform.position, OurFlag.transform.position);
                if (Distancetoflag > 50)
                {

                    //add steering force towards OURFLAG;
                }

            }
        }

        public IEnumerator TotalCharacterCheck()
        {
            IsChecking = true;
            yield return new WaitForSeconds(3.0f);
            TotalCharacterCheckV();
        }

        public void TotalCharacterCheckV()
        {
            TotalCharacters.Clear();
            foreach (CharInfo Character in Characters) //this foreach function is for adding each element of the Characters array into the totalcharacters list
            {
                TotalCharacters.Add(Character);
            }
            IsChecking = false;
        }

        public IEnumerator ChartoEnemy()
        {
            IsAdding = true;
            yield return new WaitForSeconds(3.0f);
            ChartoEnemyV();
        }

        public void ChartoEnemyV()
        {
            MyEnemies.Clear();
            foreach (CharInfo Character in TotalCharacters)
            {
                if(Character.MyTeam != this.MyTeam)
                {
                    MyEnemies.Add(Character);
                }
            }
            IsAdding = false;
        }


        #endregion

        */

    }

