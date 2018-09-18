using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

	void OnEnable()
	{
		EnemyEventManager.OnPlayerFound += ShootFunction;
	}

	void OnDisable()
	{
		EnemyEventManager.OnPlayerFound -= ShootFunction;
		
	}

	private void ShootFunction(Vector3 playerPosition)
	{
		Debug.Log (".");
	}
}
