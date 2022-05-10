using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")] 
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    private bool sliding;

    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Slide") && (horizontalInput != 0 || verticalInput != 0) && pm.grounded)
        {
            StartSlide();
        }
        
        else if (Input.GetButtonDown("Slide") && (horizontalInput != 0 || verticalInput != 0) && !pm.grounded)
        {
            StartSlideAir();
        }

        if (Input.GetButtonUp("Slide") && sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            SlidingMovement();
        }
    }

    private void StartSlide()
    {
        sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }
    
    private void StartSlideAir()
    {
        sliding = true;

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f /*|| pm.grounded*/)
        {
            rb.AddForce(inputDirection.normalized * slideForce /**0.5f*/, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        
        // if (!pm.grounded)
        // {
        //     rb.AddForce(inputDirection.normalized * slideForce * 1.5f, ForceMode.Force);
        //
        //     slideTimer -= Time.deltaTime;
        // }

        //sliding down a slope
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
        {
            StopSlide();
        }
    }

    private void StopSlide()
    {
        sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
