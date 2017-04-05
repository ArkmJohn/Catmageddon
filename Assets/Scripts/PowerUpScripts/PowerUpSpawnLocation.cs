using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawnLocation : MonoBehaviour {

    public GameObject MySpawnedPowerUp;
    public ParticleSystem MyEffectWhenSpawning;

    public void SpawnAPowerUp(GameObject myNewPowerUp)
    {
        MyEffectWhenSpawning.Play();
        myNewPowerUp.GetComponent<PowerUpEffectApplication>().Spawner = this.gameObject;
        MySpawnedPowerUp = myNewPowerUp;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
