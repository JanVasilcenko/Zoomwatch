using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")] 
    [SerializeField] public LayerMask wallMask;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] public float wallRunForce;
    [SerializeField] public float wallJumpUpForce;
    [SerializeField] public float wallJumpSideForce;
    [SerializeField] public float maxWallRunTime;
    private float wallRunTimer;
    
    [Header("Input")] 
    private float horizontalInput;
    private float verticalInput;
    
    [Header("Detection")] 
    [SerializeField] public float wallCheckDistance;
    [SerializeField] public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")] 
    private bool exitingWall;
    [SerializeField] public float exitWallTime;
    private float exitWallTimer;
    
    [Header("Gravity")]
    [SerializeField] public bool useGravity;
    [SerializeField] public float gravityCounterForce;

    [Header("References")] 
    public Transform orientation;
    public PlayerCamera camera;
    private PlayerMovement pm;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, wallMask);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, wallMask);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundMask);
    }

    private void StateMachine()
    {
        //Getting inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        //State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            //start wallrun here
            if (!pm.wallrunning)
            {
                StartWallRun();
            }

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }
            
            //wall jump
            if (Input.GetButtonDown("Jump"))
            {
                WallJump();
            }
        }
        
        //State 2 - Exiting
        else if (exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }
        
        //State 3 - None
        else
        {
            if (pm.wallrunning)
            {
                StopWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        wallRunTimer = maxWallRunTime;
        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        //apply camera effects
        camera.DoFov(90f);
        if(wallLeft)
            camera.DoTilt(-5f);
        if(wallRight)
            camera.DoTilt(5f);
    }
    
    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        
        //forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        
        //push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallForward * 100, ForceMode.Force);   
        }
        
        //weaken gravity
        if(useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }
    
    private void StopWallRun()
    {
        pm.wallrunning = false;
        
        //reset camera effects
        camera.DoFov(80f);
        camera.DoTilt(0f);
    }

    private void WallJump()
    {
        //enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;
        
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;
        
        //reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
