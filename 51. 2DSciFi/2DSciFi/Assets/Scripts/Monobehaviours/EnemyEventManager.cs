using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEventManager : MonoBehaviour {

	public bool playerFound;
	public Vector3 playerPos;
	public delegate void PlayerFoundAction(Vector3 playerPosition);
	public static event PlayerFoundAction OnPlayerFound;



	void FixedUpdate()
	{
		if (playerFound) {
			OnPlayerFound (playerPos);
		} 
	}
}
