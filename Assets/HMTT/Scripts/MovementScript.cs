using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [SerializeField]
    private float accFactor;
    [SerializeField]
    private float maxSpeed;
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
    private bool isOnWall;
    private bool isActive { get; set; }

    [SerializeField]
    private LayerMask groundMask;

    public Vector3 getVelocity()
    {
        return rb.velocity;
    }

    // Start is called before the first frame update
    void Start()
    {
        col = this.gameObject.GetComponent<Collider>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        canDash = true;
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(canDoubleJump);
        applyGravity();
        CheckIfGround();
        CheckWall();
        CheckDash();
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    Jump();
                }
                else if (!isGrounded && !isOnWall) 
                {
                    DoubleJump();
                } else
                {
                    WallJump();
                }
            }
            Move();
        }
        
    }


    void applyGravity() //1
    {
        rb.AddForce(transform.up*gravityForce, ForceMode.Acceleration); //apply force in the direction of gravity
    }

    void Move() //2
    {
        Vector3 force = new Vector3(Input.GetAxis("Horizontal") * accFactor, 0, 0);
        rb.AddForce(force, ForceMode.Impulse);

        Vector3 clampedVel = rb.velocity;
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            clampedVel = new Vector3(Mathf.Sign(clampedVel.x) * maxSpeed, rb.velocity.y, 0);
        }
        rb.velocity = clampedVel;
        //move function should move the player using rb.AddForce rather than transform.Translate
        //can use Input.GetAxis("Horizontal")
    }

    void Jump() //3
    {
        //use AddForce()
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void DoubleJump() //4
    {
        if (canDoubleJump) 
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            canDoubleJump = false;
        }
    }

    void WallJump() //3
    {

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

        float originalAccFactor = accFactor;
        accFactor *= 1.5f;

        float dashTimeElapsed = 0f;
        Debug.Log("Dashing for " + dashDuration);
        while (dashTimeElapsed < dashDuration)
        {
            Vector3 dashForce = new Vector3(dashDirection * accFactor, 0, 0);
            rb.AddForce(dashForce, ForceMode.Impulse);

        Vector3 clampedVel = rb.velocity;
        if (Mathf.Abs(clampedVel.x) > dashSpeed)
        {
            clampedVel = new Vector3(Mathf.Sign(clampedVel.x) * dashSpeed, clampedVel.y, clampedVel.z);
            Debug.Log("Greater");
        }
        rb.velocity = clampedVel;

        dashTimeElapsed += Time.deltaTime;
        yield return null;
        }

        Debug.Log("Dash done");
        accFactor = originalAccFactor;
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

        float originalAccFactor = accFactor;
        accFactor *= 1.3f;

        float dashTimeElapsed = 0f;
        Debug.Log("Air Dash");
        while (dashTimeElapsed < dashDuration)
        {
            Vector3 dashForce = new Vector3(dashDirection * accFactor, 0, 0);
            rb.AddForce(dashForce, ForceMode.Impulse);
            rb.AddForce(transform.up * gravityForce * (-0.1f), ForceMode.Acceleration); //slows gravity

        Vector3 clampedVel = rb.velocity;
        if (Mathf.Abs(clampedVel.x) > dashSpeed)
        {
            clampedVel = new Vector3(Mathf.Sign(clampedVel.x) * dashSpeed, clampedVel.y, clampedVel.z);
            rb.AddForce(transform.up * gravityForce * (-0.1f), ForceMode.Acceleration);
            Debug.Log("Greater");
        }
        rb.velocity = clampedVel;

        dashTimeElapsed += Time.deltaTime;
        yield return null;
        }

        accFactor = originalAccFactor;
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    void CheckWall() //3
    {
        //when the player is stuck to the wall, they should slowly slide down
        //change gravityForce
        RaycastHit hit; //get a hit variable to store the hit information
       
        Vector3 spherePos = transform.position;
        if (Physics.Raycast(spherePos, -transform.right, out hit, .2f)){
            isWallRight = false;
            isWallLeft = true;
            gravityForce = wallGravity;
        }
        else if(Physics.Raycast(spherePos, transform.right, out hit, .2f)){
            isWallLeft = false;
            isWallRight= true;
            gravityForce = wallGravity;
        }
        else{
            isWallRight = false;
            isWallLeft = false;
            gravityForce = baseGravity;
        }
    }

    void CheckIfGround() //2
    {
        float offsetHeight = col.bounds.extents.y;
        Vector3 rayPos = transform.position + Vector3.down * offsetHeight;
        isGrounded = Physics.CheckSphere(rayPos, 0.3f, groundMask);
        if (isGrounded)
        {
            canDoubleJump = true;
            canDash = true;
        }
    }
}
