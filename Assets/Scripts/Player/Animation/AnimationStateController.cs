using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
    public PlayerMovement pm;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Sprint") && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && pm.grounded)
        {
            animator.SetBool("isSprinting", true);
            animator.SetBool("isIdle", false);
        }

        if (Input.GetButtonUp("Sprint"))
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isSprinting", false);
        }
        
        if (Input.GetButton("Shoot") || (Input.GetAxis("Shoot") != 0) || Input.GetButtonDown("Shoot") || (Input.GetAxis("Shoot") != 0))
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isSprinting", false);
        }
        
        if (Input.GetButton("Grapple"))
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isSprinting", false);
        }
    }

    public void SwitchWeapon()
    {
        Debug.Log("I Switched weapon");
    }
}
