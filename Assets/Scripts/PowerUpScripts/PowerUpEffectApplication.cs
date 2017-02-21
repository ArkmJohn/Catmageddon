using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffectApplication : MonoBehaviour {

	//Handles PowerUp Effect Distribution
	//Handles duration

	bool miceSpawn;
	bool invul;
	int powerUpID;
	float speedIncrease;
	int heal;

	// Use this for initialization
	void Start () {
		
	}

	public bool ApplyInvulnerability()
	{
		return true;
	}

	public float HealPlayer()
	{
		return 0;
	}

	public float IncreaseSpeed()
	{
		return 0;
	}

	public bool TriggerMiceSpawn()
	{
		return true;
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
