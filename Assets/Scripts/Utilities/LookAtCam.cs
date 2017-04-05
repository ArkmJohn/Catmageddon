using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Looks at the camera
public class LookAtCam : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.LookAt(Camera.main.gameObject.transform);
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
