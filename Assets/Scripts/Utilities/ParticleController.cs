using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public float deathTimer = 0.5f;
    bool isEnabled = false;

	// Update is called once per frame
	void Update () {
        if (isEnabled)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
                Destroy(this.gameObject);
        }
	}
    void DestroyMe()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
    void OnEnable()
    {
        isEnabled = true;
    }
}
