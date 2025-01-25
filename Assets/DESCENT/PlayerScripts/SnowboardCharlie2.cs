using System;
using UnityEngine;

public class SnowboardCharlie2 : MonoBehaviour
{
    [Header("Fields")]
    public float autoSpeed;
    public float fasterAutoSpeed;
    public float sideLeanForce;
    public float tiltingFactor;
    public float gravityScale;
    public float jumpForce;
    public float airSpinSpeed;
    public float airSommersault;
    public float rotationAdjustionFactor;
    public float airTimeStart;
    [Header("References")]
    public Animator animator;
    public Grounded playerGrounded;
    public Rigidbody rb;
    private float currentAutoSpeed;
    public Vector3 rbv;
    public TrailRenderer thickTrail;
    public TrailRenderer thinTrail;
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
                    m_FixedUpdateHandler = FixedGroundedBehavior;
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
    private UpdateHandler m_FixedUpdateHandler;
    private Quaternion groundTargetRotation;
    public enum PlayerState
    {
        Grounded,
        Midair
    }
    public enum GroundState
    {
        Sliding, //no input
        Carving, //SA, SD
        Drifting, //A, D
        Shortturning, //WA, WD
        Straightening //W
    }
    public GroundState currGroundState;
    public float stateTimeCounter;
    public Vector3 moveDirection;
    private GameManager manager;

    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        manager.Setup(rb);
        currentAutoSpeed = autoSpeed;
        CurrentState = PlayerState.Midair;
    }
    float xInput;
    float yInput;
    float animCrouchBlend;
    // Update is called once per frame
    void Update()
    {
        //Animations
        if (m_CurrentState == PlayerState.Midair)
            animCrouchBlend = Mathf.Lerp(animCrouchBlend, 1f, 3f * Time.deltaTime);
        else if (Input.GetButton("Jump"))
            animCrouchBlend = Mathf.Lerp(animCrouchBlend, 1f, 6f * Time.deltaTime);
        else
            animCrouchBlend = Mathf.Lerp(animCrouchBlend, 0f, 7f * Time.deltaTime);
        if (animator != null)
            animator.SetFloat("CrouchBlend", animCrouchBlend);

        // Gravity
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        m_UpdateHandler?.Invoke();
    }
    void FixedUpdate() {
        rbv = rb.velocity;
        stateTimeCounter++;
        m_FixedUpdateHandler?.Invoke();
    }

    private void OnGrounded() {
        thinTrail.emitting = true;
        manager.ScoreTrick(false);
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.FadeIn("Background", .2f);
        audioManager.FadeOut("Strong Wind", .1f);
    }
    private void GroundedBehavior()
    {
        //print("IN Ground");
        // Get player input for movement
        if (yInput < 0) {
            if (xInput != 0 && yInput < 0 && currGroundState != GroundState.Carving) {
                setTrailThick(true);
                currGroundState = GroundState.Carving;
                stateTimeCounter = 0;
            } else if (xInput == 0) {
                setTrailThick(false);
                currGroundState = GroundState.Straightening;
                stateTimeCounter = 0;
            }
        } else if (yInput == 0) {
            if (xInput != 0 && currGroundState != GroundState.Drifting){
                setTrailThick(true);
                currGroundState = GroundState.Drifting;
                stateTimeCounter = 0;
            } else if (xInput == 0 && currGroundState != GroundState.Sliding) {
                setTrailThick(false);
                currGroundState = GroundState.Sliding;
                stateTimeCounter = 0;
            }
        } else if (yInput > 0) {
            if (xInput != 0 && currGroundState != GroundState.Shortturning) {
                setTrailThick(true);
                currGroundState = GroundState.Shortturning;
                stateTimeCounter = 0;
            } else if (xInput == 0 && currGroundState != GroundState.Sliding) {
                setTrailThick(false);
                currGroundState = GroundState.Sliding;
                stateTimeCounter = 0;
            }
        }
        
        AlignTerrain();

        // Jumps
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            float jumpFactor = 1;
            if (currGroundState == GroundState.Straightening) {
                jumpFactor = 1 + stateTimeCounter * 0.01f;
            }
            rb.AddForce(new Vector3(0f, jumpForce * jumpFactor * 10f, 0f), ForceMode.Impulse);
            //print(jumpFactor);
            animCrouchBlend = 0f;
        }
    }

    private void FixedGroundedBehavior()
    {
        AddSlopeVelocity();
        rb.velocity += transform.right * xInput * sideLeanForce * Time.fixedDeltaTime;
        float xVel = rb.velocity.x;
        currentAutoSpeed = Mathf.Clamp(currentAutoSpeed + Time.fixedDeltaTime * yInput, autoSpeed, fasterAutoSpeed);
        
        rb.velocity += transform.forward * currentAutoSpeed * Time.fixedDeltaTime;
        rb.velocity += transform.forward * Time.fixedDeltaTime;
    }   

    private void OnMidair()
    {
        thinTrail.emitting = false;
        thickTrail.emitting = false;
        manager.ScoreTrick(true);
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.FadeOut("Background", .2f);
        audioManager.FadeIn("Strong Wind", .1f);
        rb.AddTorque(transform.up * airSommersault * xInput * tiltingFactor * 10f, ForceMode.Impulse);
        rb.AddTorque(transform.right * airSpinSpeed * yInput * tiltingFactor * 10f, ForceMode.Impulse);
        
    }
    private void MidairBehavior()
    {
        //print("IN AIR");
        rb.velocity += 1.5f * Vector3.down * gravityScale * Time.deltaTime;
        rb.velocity += transform.right * xInput * sideLeanForce * Time.deltaTime * 0.5f;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.forward, rb.velocity) * transform.rotation;
        rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
            Time.fixedDeltaTime * tiltingFactor * 0.5f);
        float maxAngVel = 7f;
        rb.AddTorque(transform.right * yInput * airSpinSpeed * Time.fixedDeltaTime * (maxAngVel - rb.angularVelocity.x) / maxAngVel, ForceMode.VelocityChange);
        rb.AddTorque(transform.up * xInput * airSommersault * Time.fixedDeltaTime * (maxAngVel - rb.angularVelocity.y) / maxAngVel, ForceMode.VelocityChange);
    
        
    }
    private void AlignTerrain()
    {
        if (Physics.Raycast(transform.position + transform.forward, Vector3.down, out RaycastHit hitInfo, 5f, playerGrounded.layerMask))
        {
            groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            if (currGroundState == GroundState.Straightening) {
                transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, Time.deltaTime * tiltingFactor);  
            }
        }
        // Flat Rotation
        groundTargetRotation = Quaternion.FromToRotation(transform.forward, Vector3.right * xInput) * transform.rotation;
        float hity = hitInfo.normal.y;
        float hitz = hitInfo.normal.z;
        float hitx = hitInfo.normal.x;
        float hypoxz = hitx * hitx + hitz * hitz;
        float hypo = hity * hity + hitz * hitz;
        float theta = Mathf.Atan(hity/hitz) * Mathf.Rad2Deg;
        float phi = Mathf.Atan(hitx/hitz) * Mathf.Rad2Deg;
        //print(phi);
        float newAngle = (theta + 45) * Mathf.Deg2Rad;
        float pos = hitInfo.normal.x/Math.Abs(hitInfo.normal.x);
        Vector3 normal2 = new Vector3(hitInfo.normal.x, hypo * Mathf.Sin(newAngle), hypo * Mathf.Cos(newAngle));


        if (currGroundState == GroundState.Sliding) {
            groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, rotationAdjustionFactor * Time.fixedDeltaTime);
        }
        if (currGroundState == GroundState.Carving) {
            // print(transform.up);
            // print(transform.TransformDirection(-1, 1,0));
            if (hitInfo.normal.x < -0.5f) { 
                groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            } else {
                groundTargetRotation = Quaternion.FromToRotation(transform.up, normal2) * transform.rotation;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, tiltingFactor * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Euler(groundTargetRotation.x, 90 * xInput, groundTargetRotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(groundTargetRotation.x, 90 * xInput, groundTargetRotation.z), Time.fixedDeltaTime * tiltingFactor);
        }
        if (currGroundState == GroundState.Drifting) {
            if (hitInfo.normal.x < 0) { 
                groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            } else {
                groundTargetRotation = Quaternion.FromToRotation(transform.up, normal2) * transform.rotation;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, tiltingFactor * Time.fixedDeltaTime);
            //transform.rotation = Quaternion.Euler(groundTargetRotation.x, 90 * xInput, groundTargetRotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(groundTargetRotation.x, 90 * xInput, groundTargetRotation.z), Time.fixedDeltaTime * tiltingFactor);
        //     groundTargetRotation = Quaternion.FromToRotation(transform.TransformDirection(1, 1, 0), normal2) * transform.rotation;
        //     transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, rotationAdjustionFactor * Time.fixedDeltaTime);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(groundTargetRotation.x, 90 * xInput, groundTargetRotation.z), Time.fixedDeltaTime * tiltingFactor);
            }
        if (currGroundState == GroundState.Shortturning) {
            groundTargetRotation = Quaternion.FromToRotation(transform.TransformDirection(-0.75f * xInput, 1, 0), hitInfo.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, rotationAdjustionFactor * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(groundTargetRotation.x, 45 * xInput, groundTargetRotation.z), Time.fixedDeltaTime * tiltingFactor);
        }
        if (currGroundState == GroundState.Straightening) {
            groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, groundTargetRotation, rotationAdjustionFactor * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(groundTargetRotation.x, 0 * xInput, groundTargetRotation.z), Time.fixedDeltaTime * tiltingFactor);
        }
    }

    private void AddSlopeVelocity() {
        float directionalForce = 0f;
        float angle = 0f;

        if (Physics.Raycast(transform.position + Vector3.up + transform.forward, Vector3.down, out RaycastHit hitInfo, 5f, playerGrounded.layerMask))
        {
            groundTargetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            angle = Vector3.Angle(hitInfo.normal, Vector3.up); // Angle from ground up
            directionalForce = gravityScale * Mathf.Abs(Mathf.Sin(angle));
        }
        // Flat Rotation
        groundTargetRotation = Quaternion.FromToRotation(transform.forward, (Vector3.right * xInput)) * transform.rotation;

        if (currGroundState == GroundState.Carving) {
            if (stateTimeCounter < 100) {
                //rb.velocity += rb.velocity * -1f * Mathf.Abs(xInput) * Time.fixedDeltaTime;
                rb.AddForce(rb.velocity * -1f * Mathf.Abs(xInput));
            } 
        }
        if (currGroundState == GroundState.Drifting) {
            if (stateTimeCounter < 50) {
                rb.AddForce(rb.velocity *0.4f * Mathf.Abs(xInput));
            } else {
                rb.AddForce(rb.velocity *-0.5f * Mathf.Abs(xInput));
            }
        }
        if (currGroundState == GroundState.Shortturning) {
            if (stateTimeCounter < 100) {
                rb.AddForce(rb.velocity *0.3f * Mathf.Abs(xInput));
            }
        }
        // Diagonal Gravity
        rb.velocity += directionalForce * Vector3.down * Time.fixedDeltaTime;
        // Horizontal "Gravity" to make the player go up slopes
        if (Mathf.Sin(angle) < 0)
        {
            rb.velocity += Mathf.Abs(Mathf.Cos(angle)) * transform.forward * Time.fixedDeltaTime * currentAutoSpeed;
        }
    }

    private void setTrailThick(bool thick)
    {
        if (thick)
        {
            thinTrail.emitting = false;
            thickTrail.emitting = true;
        }
        else
        {
            thickTrail.emitting = false;
            thinTrail.emitting = true;
        }
    }
}