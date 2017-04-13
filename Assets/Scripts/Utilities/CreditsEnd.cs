using UnityEngine;
using System.Collections;

public class CreditsEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	   if (Input.GetKey(KeyCode.Escape))
        {
            End();
        }
	}

    public void End()
    {
        Application.LoadLevel("MainMenu");
    }
}
