using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	public void Update()
	{
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			Fire ();
		}
	}

	void Fire()
	{

		var bullet = (GameObject)Instantiate (
			             bulletPrefab,
			             bulletSpawn.position,
			             bulletSpawn.rotation);
<<<<<<< HEAD
		Destroy (bullet, 3.0f);
=======
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * 2000);
		//Destroy (bullet, 3.0f);
>>>>>>> 1a776acd963e8ee7b835c1ce142d3cb446b5ca39
	}
}
