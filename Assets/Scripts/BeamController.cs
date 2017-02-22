using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : ProjectileController {

    //public float damage = 0;

    void OnTriggerStay(Collider col)
    {
        //if (col.gameObject.GetComponent<Character>() != null)
            //col.gameObject.GetComponent<Character>().GetDamaged(damage);

    }
}
