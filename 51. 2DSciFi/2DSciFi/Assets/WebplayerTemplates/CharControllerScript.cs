using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



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

    private Rigidbody rBody;
    private bool facingRight;
    private bool grounded;
    private int jumpCount;
    private CapsuleCollider capsuleCollider;    
    private bool upPressed;
    private float input;   

	void Start () 
    {
        rBody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        facingRight = true;
        jumpCount = 0;
	}

    void FixedUpdate()
    {
        GroundCheck();
        if (Mathf.Abs(input) > float.Epsilon && (grounded || advancedSettings.airControl))
        {
            rBody.velocity = new Vector2(input*movementSettings.MaxSpeed, rBody.velocity.y);
        }

        if (input > 0f && !facingRight)
        {            
            Flip();
        }
        else if(input<0f&&facingRight)
        {            
            Flip();
        }     
    }

	void Update () 
    {
        input=GetInput();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upPressed = true;
            JumpFunction();

        }
	}

    private float GetInput()
    {
        float input = Input.GetAxis("Horizontal");        
        return input;
        
    }

    private void JumpFunction()
    {
        if (grounded&&rBody.velocity.y<=0) {
            jumpCount = 0;
        }

        if (upPressed && jumpCount < 2)
        {       
            rBody.velocity = new Vector2(rBody.velocity.x, 0f);
            rBody.AddForce(new Vector2(0f, movementSettings.JumpForce), ForceMode.Impulse);
            jumpCount += 1;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);

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
