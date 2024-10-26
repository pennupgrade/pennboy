using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Game level;

    private CharacterController controller;

    private Vector3 playerVelocity = new Vector3(0,0,0);
    private bool groundedPlayer;
    private float basePlayerSpeed = 5.0f;

    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private float jumpVelocity;

    //private float playerMass = 120;

    private float mouseSensitivity = 1;

    private float speedUp = 2f;

    private float interactDistance = 3f;

    private KeyCode runKey = KeyCode.LeftShift;
    private KeyCode failKey = KeyCode.F;
    private KeyCode interactKey = KeyCode.Mouse0;

    // is null if can't interact, otherwise can interact
    PushablePullableObject interactableObject = null;

    private void Start()
    {
        jumpVelocity = Mathf.Sqrt(-2 * gravityValue * jumpHeight);
        controller = gameObject.GetComponent<CharacterController>();
        // set the skin width appropriately according to Unity documentation: https://docs.unity3d.com/Manual/class-CharacterController.html
        controller.skinWidth = 0.1f * controller.radius;
    }

    void Update()
    {
        // modify player velocity
        jumpHelper();
        horizontalMovementHelper();
        // move player
        controller.Move(playerVelocity * Time.deltaTime);

        
        rotationHelper();
        
    }


    void jumpHelper() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer) {
            playerVelocity.y += jumpVelocity;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
    }


    void horizontalMovementHelper() {
        playerVelocity.x = 0;
        playerVelocity.z = 0;
        if (Input.GetKey(runKey)) {
            playerSpeed = basePlayerSpeed * speedUp;
        } else {
            playerSpeed = basePlayerSpeed;
        }
        if (Input.GetKey(failKey)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(5);
        }
        playerVelocity += (gameObject.transform.right * Input.GetAxis("Horizontal") + gameObject.transform.forward * Input.GetAxis("Vertical")) * playerSpeed;
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
        interactableObject = null;
        if (Physics.Raycast(origin, dir, out hit, interactDistance)) {
            interactableObject = hit.collider.gameObject.GetComponent<PushablePullableObject>();
            if (interactableObject != null) {
                // TODO: ADD PUSH/PULL INDICATOR TO HUD!!!!!
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        // TODO: REWORK THIS SECTION!
        if (hit.rigidbody != null) {
            Vector3 horizontalDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            Vector3 force = horizontalDir * 100000;
            hit.rigidbody.AddForce(force);

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
}
