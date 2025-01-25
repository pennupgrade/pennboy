using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Game level;

    private CharacterController controller;

    private Vector3 playerVelocity = new Vector3(0,0,0);
    private bool groundedPlayer;
    public static float basePlayerSpeed = 5.0f;

    public static float speedUp = 2.5f;

    // time to run from standstill
    private static float timeToRun = 2;

    private float playerSpeed = 0;

    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private float jumpVelocity;

    //private float playerMass = 120;

    public static float mouseSensitivity = 1;


    private float interactDistance = 8f;


    private float maxSpeed;

    private float defaultFieldOfView;
    private float fieldOfViewMultiplier = 1.18f;
    private float fastFieldOfView;


    private KeyCode runKey = KeyCode.LeftShift;
    private KeyCode failKey = KeyCode.F;
    private KeyCode pushKey = KeyCode.Mouse0;
    private KeyCode pullKey = KeyCode.Mouse1;


    private void Start()
    {
        jumpVelocity = Mathf.Sqrt(-2 * gravityValue * jumpHeight);
        controller = gameObject.GetComponent<CharacterController>();
        // set the skin width appropriately according to Unity documentation: https://docs.unity3d.com/Manual/class-CharacterController.html
        controller.skinWidth = 0.1f * controller.radius;
        maxSpeed = Player_Movement.basePlayerSpeed * Player_Movement.speedUp;
        defaultFieldOfView = Camera.main.fieldOfView;
        fastFieldOfView = defaultFieldOfView * fieldOfViewMultiplier;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    }

    void Update()
    {
        // modify player velocity
        jumpHelper();
        horizontalMovementHelper();
        // move player
        controller.Move(playerVelocity * Time.deltaTime);

        // interact with objects
        pushPullRaycast();

        
        rotationHelper();
        
    }


    void jumpHelper() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer) {
            playerVelocity.y += jumpVelocity;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
    }


    void horizontalMovementHelper() {
        playerVelocity.x = 0;
        playerVelocity.z = 0;

        float diffFOV = math.abs(fastFieldOfView - defaultFieldOfView);

        float hSpeed = 0.0f;
        float vSpeed = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            vSpeed += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vSpeed -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            hSpeed -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            hSpeed += 1.0f;
        }

        if (Input.GetKey(runKey) && vSpeed > 0) {
            playerSpeed = Mathf.MoveTowards(playerSpeed, maxSpeed, maxSpeed * Time.deltaTime / timeToRun);
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, fastFieldOfView, diffFOV * Time.deltaTime / timeToRun);
        } else {
            playerSpeed = Mathf.MoveTowards(playerSpeed, basePlayerSpeed, maxSpeed * Time.deltaTime / timeToRun);
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, defaultFieldOfView, diffFOV * Time.deltaTime / timeToRun);
        }
        if (Input.GetKey(failKey)) {
            int returnTo = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(returnTo);
            PlayerPrefs.SetInt("returnTo", returnTo);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(5);
        }
        playerVelocity += Vector3.Normalize(gameObject.transform.right * hSpeed + gameObject.transform.forward * vSpeed) * playerSpeed;
    }


    void rotationHelper() {
        // Rotates the camera and character object
        float rotX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float rotY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
        gameObject.transform.Rotate(0, rotX, 0);
        Camera.main.transform.Rotate(rotY, 0, 0);
        if (Camera.main.transform.localEulerAngles.y == 180 && Camera.main.transform.localEulerAngles.z == 180) {
            float diffBetweenUpDir = Mathf.Abs(270 - Camera.main.transform.localEulerAngles.x);
            float diffBetweenDownDir = Mathf.Abs(90 - Camera.main.transform.localEulerAngles.x);
            if (diffBetweenDownDir <= diffBetweenUpDir) {
                Camera.main.transform.localEulerAngles = new Vector3(90, 0, 0);
            } else {
                Camera.main.transform.localEulerAngles = new Vector3(270, 0, 0);
            }
        }
        gameObject.transform.Rotate(0, rotX, 0);
    }

    void pushPullRaycast() {
        RaycastHit hit;
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        if (Physics.Raycast(origin, dir, out hit, interactDistance)) {
            PushablePullableObject interactableObject = hit.collider.gameObject.GetComponent<PushablePullableObject>();
            if (interactableObject != null) {
                // TODO: ADD PUSH/PULL INDICATOR TO HUD!!!!!
                dir.y = 0;
                dir.Normalize();
                if (Input.GetKeyDown(pushKey)) {
                    interactableObject.push(hit.point, playerVelocity, dir);
                } else if (Input.GetKeyDown(pullKey)) {
                    interactableObject.pull(hit.point, playerVelocity, dir);
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        // TODO: REWORK THIS SECTION!
        if (hit.rigidbody != null) {
            Vector3 horizontalDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3 force = horizontalDir * playerVelocity.magnitude * 3000 * Time.fixedDeltaTime;
            hit.rigidbody.AddForce(force, ForceMode.Impulse);

            // property damage from player
            PropertyDamageCollider col = hit.gameObject.GetComponent<PropertyDamageCollider>();
            if (col != null) {
                int damage = col.calculateDamage(force.magnitude);
                if (damage != 0) {
                    level.reduceBudget(damage);
                }
            }
        }
    }

    public void SetMouseSensitivity(float sensitivity) {
        mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity); // Save to PlayerPrefs
    }
}
