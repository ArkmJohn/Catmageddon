using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : ProjectileController {

    [SerializeField]
    float deathTimer = 2f;
    public GameObject destroyParticle;
    public float forceVal = 5000;
    //public float damage = 0;
    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.GetComponent<Character>() != null)
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * forceVal);
            //col.gameObject.GetComponent<Character>().GetDamaged(damage);
        }
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        GameObject clone = Instantiate(destroyParticle, transform.position, Quaternion.identity);
        clone.transform.SetParent(null);
    }
}
