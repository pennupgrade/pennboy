using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour
{
    [Header("Fields")]
    public float autoSpeed;

    public float fasterAutoSpeed;

    public float sideLeanForce;

    public float tiltingFactor;

    public float gravityScale;

    public float jumpForce;

    public float rotationAdjustionFactor;

    [Header("References")]
    public Grounded playerGrounded;
    public Rigidbody rb;


    private float currentAutoSpeed;

    public PlayerState CurrentState
    {
        get => m_CurrentState;
        set
        {
            switch (value)
            {
                case PlayerState.Grounded:
                    OnGrounded();
                    m_UpdateHandler = GroundedBehavior;
                    break;
                case PlayerState.Midair:
                    OnMidair();
                    m_UpdateHandler = MidairBehavior;
                    break;
                default:
                    break;
            }

            m_CurrentState = value;
        }
    }
    private PlayerState m_CurrentState;

    private delegate void UpdateHandler();
    private UpdateHandler m_UpdateHandler;

    public enum PlayerState
    {
        Grounded,
        Midair
    }


    // Start is called before the first frame update
    void Start()
    {
        currentAutoSpeed = autoSpeed;

        CurrentState = PlayerState.Midair;
    }


    float xInput;
    float yInput;
    // Update is called once per frame
    void Update()
    {
        // Gravity
        //
        //rb.velocity += transform.forward * autoSpeed * Time.deltaTime;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        m_UpdateHandler?.Invoke();
    }

    private void OnGrounded()
    {


    }
    private void GroundedBehavior()
    {
        print("IN Ground");
        // Get player input for movement



        SetSlopeSlideVelocity();
        rb.velocity += transform.right * xInput * sideLeanForce * Time.deltaTime;

        float xVel = rb.velocity.x;

        currentAutoSpeed = Mathf.Clamp(currentAutoSpeed + Time.deltaTime * yInput, autoSpeed, fasterAutoSpeed);

        rb.velocity += (transform.forward * yInput * currentAutoSpeed * Time.deltaTime);
        rb.velocity += (transform.forward * Time.deltaTime);


        // Jumps
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += new Vector3(0f, jumpForce, 0f);
        }
    }

    private void OnMidair()
    {
        rb.AddTorque(transform.up * xInput * tiltingFactor * 10f, ForceMode.Impulse);

        rb.AddTorque(transform.right * yInput * tiltingFactor * 10f, ForceMode.Impulse);

    }
    private void MidairBehavior()
    {
        print("IN AIR");
        rb.velocity += Vector3.down * gravityScale * Time.deltaTime;
        rb.velocity += transform.right * xInput * sideLeanForce * Time.deltaTime * 0.5f;

        //rb.rotation = Quaternion.RotateTowards(transform.rotation, 
        //    Quaternion.AngleAxis(Time.deltaTime * 10, rb.velocity),
        //    Time.deltaTime * 100f);

        //Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, (Vector3.right * xInput)) * transform.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
        //    Time.fixedDeltaTime * tiltingFactor);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, rb.velocity) * transform.rotation;
        rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
            Time.fixedDeltaTime * tiltingFactor * 0.5f);


        float maxAngVel = 7f;

        if (rb.angularVelocity.magnitude < maxAngVel)
        {
            rb.AddTorque(transform.right * yInput * tiltingFactor * Time.fixedDeltaTime * (maxAngVel - rb.angularVelocity.x) / maxAngVel, ForceMode.VelocityChange);
            rb.AddTorque(transform.up * xInput * tiltingFactor * Time.fixedDeltaTime * (maxAngVel - rb.angularVelocity.y) / maxAngVel, ForceMode.VelocityChange);
        }
    }


    private void SetSlopeSlideVelocity()
    {
        float directionalForce = 0f;
        float angle = 0f;

        if (Physics.Raycast(transform.position + Vector3.up + transform.forward, Vector3.down, out RaycastHit hitInfo, 5f, playerGrounded.layerMask))
        {
            angle = Vector3.Angle(hitInfo.normal, Vector3.up); // Angle from ground up

            directionalForce = gravityScale * Mathf.Abs(Mathf.Sin(angle));

        }

        //transform.up = Vector3.Lerp(transform.up, hitInfo.normal, rotationAdjustionFactor * Time.deltaTime);

        // Flat Rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, (Vector3.right * xInput) + Vector3.forward * Mathf.Abs(xInput)) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
            tiltingFactor * Time.fixedDeltaTime * (1f - Quaternion.Angle(transform.rotation, targetRotation) / 180f));
        //rb.AddTorque(transform.up * tiltingFactor * Time.fixedDeltaTime * xInput, ForceMode.VelocityChange);

        // Adhere to normals
        targetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationAdjustionFactor * Time.fixedDeltaTime);


        // Diagonal Gravity
        rb.velocity += directionalForce * Vector3.down * Time.fixedDeltaTime;

        // Horizontal "Gravity" to make the player go up slopes
        if (Mathf.Sin(angle) < 0)
        {
            //print("YO");
            rb.velocity += Mathf.Abs(Mathf.Cos(angle)) * transform.forward * Time.fixedDeltaTime * currentAutoSpeed;
            //Debug.DrawRay(transform.position, (Mathf.Abs(Mathf.Cos(angle)) * transform.forward * Time.deltaTime * autoSpeed).normalized, Color.red);
        }

        //if (Vector3.Dot(rb.velocity, transform.right) > 0)
        //{
        //    rb.velocity -= transform.right * Time.fixedDeltaTime * 10f;
        //}


        //print(directionalForce);
    }
}