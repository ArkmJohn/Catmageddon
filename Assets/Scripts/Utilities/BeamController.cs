using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController : WeaponCollider
{
    bool isCheckedOwned = false;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isCheckedOwned)
        {
            StartCoroutine(CheckMe());
        }
	}

    IEnumerator CheckMe()
    {
        isCheckedOwned = true;
        yield return new WaitForSeconds(5);
        bool isOwned = false;
        foreach (CatInfo a in FindObjectsOfType<CatInfo>())
        {
            if (a.Beam == this.gameObject)
            {
                isOwned = true;
            }
        }
        if (!isOwned)
        {
            this.gameObject.SetActive(false);
        }
        isCheckedOwned = false;
    }

}
