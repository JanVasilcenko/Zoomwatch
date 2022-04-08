using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerMovement pm;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Sprint") /*&& pm.grounded*/)
        {
            animator.SetBool("isSprinting", true);
            animator.SetBool("isIdle", false);
        }

        if (Input.GetButtonUp("Sprint"))
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isSprinting", false);
        }
        
        // if (Input.GetButton("Jump"))
        // {
        //     animator.SetBool("isIdle", true);
        //     animator.SetBool("isSprinting", false);
        // }
        //
        // if (Input.GetButton("Grapple"))
        // {
        //     animator.SetBool("isIdle", true);
        //     animator.SetBool("isSprinting", false);
        // }
    }
}
