using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSearch : MonoBehaviour {

	public RaycastHit hitInfo;
	public Vector3 enemyPosition;
	public Vector3 enemyDirection;
    public GameObject enemyMiddleNull;
    public EnemyEventManager enemyEventManager;
	public bool foundPlayer=false;
    public bool prevFoundPlayer=false;
    public bool newlyFound = false;
    public bool newlyLost = false;


    void FixedUpdate()
	{
        enemyPosition = enemyMiddleNull.transform.position;
        enemyDirection = enemyMiddleNull.transform.forward;
        if (Physics.SphereCast (enemyPosition, 1f, enemyDirection, out hitInfo, 5f))
		{
			if (hitInfo.collider.gameObject.tag == "Player") {
                foundPlayer = true;
                enemyEventManager.playerPos = hitInfo.point;
                enemyEventManager.playerFound = true;
                if (!prevFoundPlayer && foundPlayer)
                {
                    newlyFound = true;
                }
                else
                {
                    newlyFound = false;
                }
			} else {
				enemyEventManager.playerFound = false;
                foundPlayer = false;
                if (prevFoundPlayer && !foundPlayer)
                {
                    newlyLost = true;
                }
                else
                {
                    newlyLost = false;
                }
            }
		}else 
		{
			enemyEventManager.playerFound = false;
            foundPlayer = false;
            if (prevFoundPlayer && !foundPlayer)
            {
                newlyLost = true;
            }
            else
            {
                newlyLost = false;
            }
        }
        prevFoundPlayer = foundPlayer;       
    }
    


}
