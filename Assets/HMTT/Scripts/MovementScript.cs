using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MovementScript : MonoBehaviour
{
    [SerializeField]
    private float accFactor;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float airDrag;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float doubleJumpForce;
    private bool canDoubleJump;
    [SerializeField]
    private float baseGravity;
    [SerializeField]
    private float wallGravity;
    private float gravityForce;
    private bool isGrounded;
    private bool isWallRight;
    private bool isWallLeft;
    private int gravityDir { get; set; }
    [SerializeField]
    private float dashSpeed;
    private bool isDashing;
    private bool canDash;
    [SerializeField]
    private float dashCD;
    [SerializeField]
    private float dashDuration;
    private Collider col;
    private Rigidbody rb;
    [SerializeField]
    private float wallJumpForce;

    private bool isFastFall;
    [SerializeField]
    float fastFallWall;
    [SerializeField]
    float fastFallAir;
    private bool isActive { get; set; }

    [SerializeField]
    Animator anim;

    private bool isjump;

    [SerializeField]
    private LayerMask groundMask;

    public Vector3 getVelocity()
    {
        return rb.velocity;
    }

    public int getGravityDir()
    {
        return gravityDir;
    }

    public void setGravityDir(int newGravityDir)
    {
        gravityDir = newGravityDir;
    }

    // Start is called before the first frame update
    void Start()
    {
        col = this.gameObject.GetComponent<Collider>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        gravityDir = 1;
        canDash = true;
        isActive = true;
        gravityForce = baseGravity;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfGround();
        CheckWall();
        CheckDash();
        SetGravity();
        applyGravity();
        if (isActive)
        {
            anim.SetBool("Running", (Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded));
            anim.SetFloat("MotionSpeed", Mathf.Pow((rb.velocity.x / maxSpeed), 0.1f));
            anim.SetBool("FreeFall", (rb.velocity.y < 0 && !isGrounded));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    Jump();
                }
                else if (!isGrounded && !(isWallLeft || isWallRight)) 
                {
                    DoubleJump();
                } 
                else if (!isGrounded && (isWallLeft || isWallRight)) 
                {
                    WallJump();
                }
                anim.SetBool("Jump", true);
            } else
            {
                anim.SetBool("Jump", false);
            }
            Move();
        }
    }


    void applyGravity() //1
    {
        rb.AddForce(transform.up*gravityForce*gravityDir, ForceMode.Acceleration); //apply force in the direction of gravity
    }

    void Move() //2
    {
        if (Mathf.Abs(rb.velocity.x) <= maxSpeed || Mathf.Sign(Input.GetAxis("Horizontal")) != Mathf.Sign(rb.velocity.x))
        {
            Vector3 force = new Vector3(Input.GetAxis("Horizontal") * accFactor, 0, 0);
            if (!isGrounded)
            {
                force *= airDrag;
            }
            rb.AddForce(force, ForceMode.Impulse);
        }
        //move function should move the player using rb.AddForce rather than transform.Translate
        //can use Input.GetAxis("Horizontal")
    }

    void Jump() //3
    {
        //use AddForce()
        rb.AddForce(Vector3.up * jumpForce * gravityDir, ForceMode.Impulse);
    }

    void DoubleJump() //4
    {
        if (canDoubleJump) 
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * doubleJumpForce * gravityDir, ForceMode.Impulse);
            canDoubleJump = false;
        }
    }

    void WallJump() //3
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);

        float jumpDirection = isWallRight ? -2f : 2f; // Jump away from the wall
        rb.AddForce(new Vector3(jumpDirection * wallJumpForce, jumpForce*gravityDir, 0), ForceMode.Impulse);
    }

    void SetGravity() //3
    {
        if(!isGrounded) {
            if((isWallLeft || isWallRight) && rb.velocity.y <= 0.01f)
            {
                gravityForce = wallGravity;
            }
            else{
                gravityForce = baseGravity;
            }

            if (Input.GetKey(KeyCode.S) && !isFastFall) {
                gravityForce *= (isWallLeft || isWallRight) ? fastFallWall : fastFallAir;
                isFastFall = true;
            } else {
                isFastFall = false;
            }
            if(isDashing){
                gravityForce = 0f;
            }
        }
        else{
            gravityForce = baseGravity;
        }

        
    }

    void CheckDash() //2
    {
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            StartCoroutine(Dash());
        }

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && !isGrounded)
        {
            StartCoroutine(AirDash());
        }

    }

    IEnumerator Dash() //3
    {
        //the dash should set the player's velocity to the dash velocity IF the player's original velocity was slower than the dash's velocity.
        //otherwise, use the player's original velocity
        //goign to need to store the player's original velocity in a temporary variable
        canDash = false;
        isDashing = true;
        float dashDirection = Input.GetAxisRaw("Horizontal");

        if (dashDirection == 0)
        {
            dashDirection = Mathf.Sign(rb.velocity.x); // If no input is detected, maintain the current movement direction
        }

        float dashTimeElapsed = 0f;
        //Debug.Log("Dash");
        while (dashTimeElapsed < dashDuration)
        {
            rb.velocity = new Vector3(dashDirection * dashSpeed, rb.velocity.y, 0f);
            dashTimeElapsed += Time.deltaTime;
            yield return null;
        }

       // Debug.Log("Dash done");
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    IEnumerator AirDash() //3
    {
        canDash = false;
        isDashing = true;
        float dashDirection = Input.GetAxisRaw("Horizontal");

        if (dashDirection == 0)
        {
            dashDirection = Mathf.Sign(rb.velocity.x);
        }

        float dashTimeElapsed = 0f;
        //Debug.Log("Air Dash");
        while (dashTimeElapsed < dashDuration)
        {
            rb.velocity = new Vector3(dashDirection * (dashSpeed - 2f), Mathf.Max(0, rb.velocity.y), 0f);
            dashTimeElapsed += Time.deltaTime;
            yield return null;
        }

       // Debug.Log("Air Dash Done");
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    void CheckWall() //3
    {
        //when the player is stuck to the wall, they should slowly slide down
        //change gravityForce

        float offSetWidth = col.bounds.extents.x;
        Vector3 rayPosLeft = transform.position + Vector3.left * offSetWidth;
        Vector3 rayPosRight = transform.position + Vector3.right * offSetWidth;
        if (Physics.CheckSphere(rayPosLeft, 0.2f, groundMask)){
            isWallRight = false;
            isWallLeft = true;
        }
        else if(Physics.CheckSphere(rayPosRight, 0.2f, groundMask)){
            isWallLeft = false;
            isWallRight= true;
        }
        else{
            isWallRight = false;
            isWallLeft = false;
        }
    }

    void CheckIfGround() //2
    {
        float offsetHeight = col.bounds.extents.y;
        Vector3 rayPos = transform.position + Vector3.down * offsetHeight * gravityDir;
        isGrounded = Physics.CheckSphere(rayPos, 0.1f, groundMask);
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        anim.SetBool("Grounded", isGrounded);
    }
}