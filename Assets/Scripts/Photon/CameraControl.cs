using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    #region Public Properties

    public float distance = 7.0f;
    public float height = 3.0f;
    public float heightSmoothLag = 0.3f;
    public Vector3 centerOffset = Vector3.zero;
    public bool followOnStart = false;

    Transform cameraT;
    bool isFollowing;
    float heightVel;
    float targetHeight = 100000;

    #endregion

    // Use this for initialization
    void Start ()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            InitFollowing();
        }
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (cameraT == null && isFollowing)
        {
            InitFollowing();
        }

        if (isFollowing)
        {
            //Apply();
        }

	}

    void InitFollowing()
    {
        cameraT = Camera.main.transform;
        isFollowing = true;

        //
        float oldHeightSmooth = heightSmoothLag;
        heightSmoothLag = 0.001f;

    }
}
