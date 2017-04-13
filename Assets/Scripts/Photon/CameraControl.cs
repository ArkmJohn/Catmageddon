using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    #region Public Properties

    public float distance = 7.0f;
    public float height = 3.0f;
    public Transform localPlayer;
    public float velocity = 5000;

    Transform Target;
    Transform cameraT;
    bool isFollowing; 

    #endregion

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (localPlayer != null)
        {
            if (Target == null)
            {
                CreateObject();

            }
            if(cameraT == null)
                cameraT = Camera.main.gameObject.transform;
            else
                InitFollowing();
        }

	}

    void InitFollowing()
    {
        Vector3 target = Target.position;
        cameraT.transform.position = Vector3.Lerp(cameraT.position, target, velocity);
        cameraT.transform.LookAt(localPlayer);

    }

    void CreateObject()
    {
        GameObject CameraTarget = new GameObject("TargetCamPos");

        CameraTarget.transform.position = localPlayer.position + new Vector3(0, height, -distance);
        CameraTarget.transform.SetParent(localPlayer);
        Target = CameraTarget.transform;
    }
}
