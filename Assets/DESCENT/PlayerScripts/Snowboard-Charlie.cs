using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboardCharlie : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float gravity = 10f;
    [SerializeField] float terrainBuffer = 3.5f;
    public LayerMask terrainLayer;
    private Rigidbody rb;
    private Quaternion targetRotation;
    private float horizontalInput;
    private float verticalInput;
    public float stateTimeCounter;
    private Quaternion lockedDirection;
    /**
    State 0 = Sliding
    State 1 = Carving
    State 2 = Accelerating
    State 3 = Accelerating in direction.
    **/
    public float state;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // Handle player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(-1f, 0, horizontalInput) * moveSpeed;
        rb.AddForce(new Vector3(0, -0.1f * gravity, 0));
        
        if (horizontalInput != 0 && state != 1) {
            moveDirection = new Vector3(0, 0, horizontalInput) * moveSpeed;
            state = 1;
            stateTimeCounter = 0;
        } else if (verticalInput != 0 && state != 2){
            state = 2;
            stateTimeCounter = 0;
        } else if (horizontalInput == 0 && verticalInput == 0) {
            state = 0;
            stateTimeCounter = 0;
        } 
        if (state == 1 && verticalInput != 0) {
            state = 3;
            stateTimeCounter = 0;
        }
        // Apply movement
        transform.position += moveDirection * Time.deltaTime;
        // Align player to terrain
        AlignToTerrain();
    }   

    void FixedUpdate(){
        stateTimeCounter++;
    }

    void AlignToTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
        {
            // Align player to the terrain normal
            targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            if (state == 0) {                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);  
            } 
            if (state ==1) {
                Carve();
            }
            if (state == 2) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation.x, transform.rotation.y, targetRotation.z), Time.deltaTime * turnSpeed);
                rb.AddForce(new Vector3(-1, 0, 0)* moveSpeed);
            } 
            if (state == 3) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 45 * horizontalInput, transform.rotation.z), Time.deltaTime * turnSpeed);
                Vector3 forward = transform.TransformDirection (Vector3.forward);
                rb.AddForce(forward * moveSpeed);
            }    
            //Debug.DrawRay(hit.point, hit.normal, Color.red);
            // Adjust player height to terrain height with buffer
            Vector3 targetPosition = new Vector3(transform.position.x, hit.point.y + terrainBuffer, transform.position.z);
            transform.position = targetPosition;
        }
    }
    void Carve()
    {
        //rb.MoveRotation(Quaternion.Euler(0, horizontalInput * 30, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 90 * horizontalInput, transform.rotation.z), Time.deltaTime * turnSpeed);
        if (stateTimeCounter < 100) {
            rb.AddForce(rb.velocity *-1f * Mathf.Abs(horizontalInput));
        } 
        //rb.AddForce(Vector3.up * 1f); 
    }

}
