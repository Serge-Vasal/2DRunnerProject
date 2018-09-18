using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    [Serializable]
    public class MovementSettings
    {
        public float MaxSpeed=2.0f;             
    }

    public MovementSettings movementSettings = new MovementSettings();
    public GameObject enemyMiddleNull;
    public float distanceToRandomPoint;
	public EnemyPlayerSearch enemyPlayerSearch;    

    private Rigidbody rBody;
    private CapsuleCollider capsuleCollider; 
    private Animator anim;
    private bool gunRight;
    private Vector3 randomPoint;
    private bool needToTurn;
	private GameObject currSearchGO;
	private int timeDelayBeforeShootingStart;
    private int timeDelayBeforePatrolStart;
    private bool shootingIdle=false;    
    private bool patrol;
    private bool idle;
    private float alertActivationTimer;
    private float alertDeActivationTimer;
    private bool prepareShooting;

	void Start () {        
        gunRight = false;
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        anim = GetComponentInChildren<Animator>();
		timeDelayBeforeShootingStart = 0;
        patrol = true;

        randomPoint = RandomPointFinder(enemyMiddleNull.transform.position, enemyMiddleNull.transform.forward, 3f, 5f); 
	}

    void FixedUpdate()
    {
        
        if (needToTurn)
        {
            Flip();
            randomPoint = RandomPointFinder(enemyMiddleNull.transform.position, enemyMiddleNull.transform.forward, 3f, 7f);
            
        }
        if (shootingIdle)
        {
            anim.SetBool("Shooting", true);
            anim.SetBool("Idle", true);
            anim.SetBool("Patrol", false);
        }
        else if (prepareShooting)
        {
            anim.SetBool("Shooting", false);
            anim.SetBool("PrepareShooting", true);
        }
        else if (patrol)
        {
            anim.SetBool("Shooting", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Patrol", true);
            rBody.velocity = new Vector2(gunRight ? 2f : -2f, rBody.velocity.y);
            needToTurn = DistanceToCheckPointChecker(enemyMiddleNull.transform.position, randomPoint, distanceToRandomPoint);
        }
        else if (idle)
        {
            anim.SetBool("Shooting", false);
            anim.SetBool("Idle", true);
            anim.SetBool("Patrol", false);
        }


        if (enemyPlayerSearch.newlyFound)
        {
            prepareShooting = true;
            alertActivationTimer = 0f;
            StartCoroutine("AlertActivation");
        }
        else if (enemyPlayerSearch.newlyLost)
        {            
            shootingIdle = false;
            prepareShooting = true;
            alertDeActivationTimer = 0f;
            StartCoroutine("AlertDeActivation");
        }             

    }

	void OnEnable()
	{
		EnemyEventManager.OnPlayerFound += OnAlert;
	}

	void OnDisable()
	{
		EnemyEventManager.OnPlayerFound -= OnAlert;
	}


    public Vector3 RandomPointFinder(Vector3 position,Vector3 direction,float minValue,float maxValue)
    {
        Ray randomPointRay = new Ray(position, direction);
        Vector3 randomPoint = randomPointRay.GetPoint(UnityEngine.Random.Range(minValue,maxValue));
        return randomPoint;
    }

    public bool DistanceToCheckPointChecker(Vector3 currPosition,Vector3 randomPoint,float distanceToRandomPoint)
    {
        Vector3 currPosToRandomPoint = currPosition - randomPoint;
        float sqrLength = currPosToRandomPoint.sqrMagnitude;
        float sqrDistanceToRandomPoint = distanceToRandomPoint * distanceToRandomPoint;
        bool needToTurn=false;
        if (sqrLength <=sqrDistanceToRandomPoint )
        {
            needToTurn = true;  
        }
        else
        {
            needToTurn = false;
        }
        return needToTurn;
    }

    public void Flip()
    {
        gunRight = !gunRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World); 
    }

    private void OnAlert(Vector3 playerPosition)
    {
        idle = true;
        patrol = false;
    }     
        

    IEnumerator AlertActivation()
    {

        while (timeDelayBeforeShootingStart < 51)
        {
            if (timeDelayBeforeShootingStart >= 50)
            {

                shootingIdle = true;
                timeDelayBeforeShootingStart = 0;
                yield break;                
            }

            timeDelayBeforeShootingStart += 1;
            
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AlertDeActivation()
    {

        while (timeDelayBeforePatrolStart < 201)
        {
            if (timeDelayBeforePatrolStart >= 200)
            {
                timeDelayBeforePatrolStart = 0;
                prepareShooting = false;
                patrol = true;
                yield break;
            }

            timeDelayBeforePatrolStart += 1;
            yield return new WaitForFixedUpdate();
        }
    }
    


}
