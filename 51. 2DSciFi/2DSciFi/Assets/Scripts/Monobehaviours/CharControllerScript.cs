using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CharControllerScript : MonoBehaviour { 

    [Serializable]
    public class MovementSettings
    {
        public float MaxSpeed=8.0f;
        public float JumpForce=30f;       
    }

    [Serializable]
    public class AdvancedSettings
    {
        public float groundCheckDistance=0.01f;
        public float shellOffset;
        public bool airControl;
    }

    public AdvancedSettings advancedSettings=new AdvancedSettings();
    public MovementSettings movementSettings = new MovementSettings();
    public GunMover gunMover;
    public Transform rightArm;
    public Transform leftArm;
    public Transform head;

    private Rigidbody rBody;
    private bool grounded;
    private int jumpCount;
    private CapsuleCollider capsuleCollider;    
    private bool jump;
    private float input;
    private float accelInputZ=0.0f;
    private Animator anim;
    private bool gunRight;



	void Start () 
    {        
        gunRight = true;
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        jumpCount = 0;
        anim = GetComponentInChildren<Animator>();

	}

    void FixedUpdate()
    {
        GroundCheck();
        if (Mathf.Abs(input) > float.Epsilon && (grounded || advancedSettings.airControl))
        {
            rBody.velocity = new Vector2(input*movementSettings.MaxSpeed, rBody.velocity.y);
        }
    }

	void Update () 
    {
        
        rightArm.rotation = gunMover.worldGunNull.transform.rotation;
        leftArm.rotation = gunMover.worldGunNull.transform.rotation;
        head.rotation = gunMover.worldGunNull.transform.rotation;
        gunRight = gunMover.gunRight;
        input=GetInputRun();
        if (gunRight)
        {
            anim.SetFloat("Speed",input);
        }
        if (!gunRight)
        {
            anim.SetFloat("Speed",-input);
        }
       
        jump = GetInputJump();
        if (jump)
        {            
            JumpFunction();
        }
	}

    private float GetInputRun()
    {        
        float accelInputX = Input.acceleration.x;  
        if (Mathf.Abs(accelInputX) > 0.1f)
        {            
            return accelInputX;
        }
        else
        {            
            return 0.0f;
        }        
    }

    private bool GetInputJump()
    {        
        float PrevaccelInputZ = accelInputZ;
        accelInputZ = Input.acceleration.z;
        if ((Mathf.Abs(accelInputZ) - Mathf.Abs(PrevaccelInputZ))>0.2f)
        {
            return true;
        }
        else
        {            
            return false;
        } 
    }

    private void JumpFunction()
    {
        if (grounded&&rBody.velocity.y<=0) {
            jumpCount = 0;
        }

        if (jump && jumpCount < 2)
        {       
            rBody.velocity = new Vector2(rBody.velocity.x, 0f);
            rBody.AddForce(new Vector2(0f, movementSettings.JumpForce), ForceMode.Impulse);
            jumpCount += 1;
        }
    }

    private void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsuleCollider.radius * (1.0f - advancedSettings.shellOffset),
                Vector3.down, out hitInfo, ((capsuleCollider.height / 2f) - capsuleCollider.radius) + advancedSettings.groundCheckDistance,
                ~0, QueryTriggerInteraction.Ignore))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
