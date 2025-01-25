using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    ReadyToJump,
    JumpCharging,
    Jumping, 
    InputLocked,
    GameOver
}


public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxHeight = 2f;
    [SerializeField] float distance = 2.5f;
    [SerializeField] float minJumpDuration = 1f;
    [SerializeField] float maxJumpDuration = 2f;
    [SerializeField] float minimumHeight = 5f;
    [SerializeField] PlatformManager platformManager; 
    [SerializeField] CameraController cameraController;
    
    PlayerState playerState = PlayerState.ReadyToJump;
    
    // Jumping realted private variables 
    float jumpChargeStartTime = 0f;
    float jumpChargeDuration = 0f;
    Vector3 startingPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    float jumpDuration = 0f;
    float jumpAnimationEndThreshold = 0.01f;

    int score = 0;
    private ParticleSystem chargingEffect;
    private ParticleSystem perfectJumpEffect;
    private AudioSource jumpSoundPlayer;
    private AudioSource perfectLandSoundPlayer;

    private void Start()
    {
        jumpSoundPlayer =
            GameObject.Find("IsometricCamera/JumpSoundPlayer").GetComponent<AudioSource>();
        perfectLandSoundPlayer =
          GameObject.Find("IsometricCamera/PerfectLandingSoundPlayer").GetComponent<AudioSource>();
        ParticleSystem[] effects = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].gameObject.name == "ChargingEffect") chargingEffect = effects[i];
            if (effects[i].gameObject.name == "PerfectLanding_Effect") perfectJumpEffect = effects[i];
        }
    }

    void Update()
    {
        switch(playerState)
        {
            case PlayerState.ReadyToJump:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RotatePlayerToFacePlatform();
                    jumpChargeStartTime = Time.time;
                    startingPos = transform.position;
                    playerState = PlayerState.JumpCharging; 
                }
                break;

            case PlayerState.JumpCharging:
                if (!chargingEffect.isPlaying)
                {
                    chargingEffect.gameObject.SetActive(true);
                    chargingEffect.Play();
                }
                ShrinkAnimationController.instance.Shrink(Time.deltaTime);
                // Check if player released the jump button
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    jumpSoundPlayer.Play();
                    ShrinkAnimationController.instance.ResetSize();
                    chargingEffect.gameObject.SetActive(false);
                    chargingEffect.Stop();
                    jumpChargeDuration = Time.time - jumpChargeStartTime;
                    jumpDuration = Mathf.Clamp(jumpChargeDuration, minJumpDuration, maxJumpDuration);

                    destination = CalcualteDestination(jumpChargeDuration);

                    playerState = PlayerState.Jumping;
                }
                break; 
            case PlayerState.Jumping:
                // Jumped to Pos
                if (Vector3.Distance(transform.position, destination) < jumpAnimationEndThreshold)
                {
                    // Reset transform to upright position
                    transform.position = destination;
                    transform.rotation =
                        Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);

                    // Generate new platform if jump landed 
                    if(IsPlayerOnPlatform())
                    {
                        if (platformManager.CheckPerfectLanding(transform.position))
                        {
                            perfectLandSoundPlayer.Play();
                            perfectJumpEffect.Play();
                            score += 1;
                        }
                        platformManager.JumpLanded();
                        score += 1;
                        CanvasScript.instance.UpdateScore(score);
                    } else
                    {
                        playerState = PlayerState.GameOver;
                        Rigidbody temp = gameObject.AddComponent<Rigidbody>();
                        StartCoroutine("gameOver");
                        Debug.Log("Game Over!");
                        return; 
                    }

                    // Initiates a smooth camera movement to the player's current position with an offset.
                    // Once the camera reaches its target position, the OnMoveCameraCompleted callback is triggered.
                    playerState = PlayerState.InputLocked;
                    cameraController.MoveCamera(transform.position, OnMoveCameraCompleted);
                }
                // Still going
                else
                {
                    float time = (Time.time - (jumpChargeStartTime + jumpChargeDuration));
                    transform.position = evaluate(startingPos, destination, time / jumpDuration);
                    transform.rotation *= Quaternion.Euler((Time.deltaTime / jumpDuration) * 360, 0, 0);
                }
                break; 

            // TODO: Add input cacheing while player state is "InputLocked" so that it feels responsive.
        }
    }

    private IEnumerator gameOver()
    {
        yield return new WaitForSeconds(1.5f);
        CanvasScript.instance.DisplayGameOver();
        yield return new WaitForSeconds(2f);
        CanvasScript.instance.RestartScene();
    }

    /// <summary>
    /// Rotates the player character to face the next platform based on its direction.
    /// If the next platform is on the left, the player faces forward with no rotation.
    /// If the next platform is on the right, the player rotates 90 degrees to the right.
    /// </summary>
    public void RotatePlayerToFacePlatform()
    {
        PlatformDirection nextPlatformDirection = platformManager.GetNextPlatformDirection();
        if (nextPlatformDirection == PlatformDirection.Left)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (nextPlatformDirection == PlatformDirection.Right)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    private Vector3 evaluate(Vector3 startPos, Vector3 endPos, float t)
    {
        Vector3 midPos = (endPos + startPos)/2;
        midPos.y = Vector3.Distance(startPos, endPos) * maxHeight;
        if (midPos.y < minimumHeight) midPos.y = minimumHeight;
        Vector3 ab = Vector3.Lerp(startPos, midPos, t);
        Vector3 bc = Vector3.Lerp(midPos, endPos, t);
        return Vector3.Lerp(ab, bc, t);
    }

    private void OnDrawGizmos()
    {
        if (startingPos == null || destination == null) return;
        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(evaluate(startingPos,destination, i / 20f), 0.1f);
        }
    }

    // Callback for platform manager to call at the end of platform animations 
    private void OnMoveCameraCompleted()
    {
        playerState = PlayerState.ReadyToJump;
    }

    private Vector3 CalcualteDestination(float jumpChargeDuration)
    {
        Vector3 destination = transform.position +
            transform.forward * jumpChargeDuration * distance;

        // Calculate the player's destination for the jump based on the charge duration.
        // The player jumps in the forward direction by a distance proportional to the charge.
        // To ensure the player always lands on the center axis of the next platform, adjust
        // the destination's alignment based on the platform's spawn direction (left or right).
        // If the platform spawns to the left, align the player along the x-axis; otherwise, align along the z-axis.
        Vector3 nextPlatformPosition = platformManager.GetNextPlatformPosition(); 
        if(platformManager.GetNextPlatformDirection() == PlatformDirection.Left)
        {
            destination.x = nextPlatformPosition.x; 
        } else
        {
            destination.z = nextPlatformPosition.z;
        }

        return destination; 
    }

    /// <summary>
    /// Checks if the player has successfully landed on the next platform by comparing the horizontal distance 
    /// (x and z coordinates only) between the player's position and the platform's center to the platform's radius.
    /// Returns true if the player is within the bounds of the platform, indicating a successful landing.
    /// </summary>
    /// <returns>True if the player has landed on the platform, false otherwise.</returns>
    private bool IsPlayerOnPlatform()
    {
        float platformRadius = platformManager.GetNextPlatformRadius();
        Vector3 playerPositionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 platformPositionXZ = new Vector3(platformManager.GetNextPlatformPosition().x, 0, platformManager.GetNextPlatformPosition().z);
        float distanceToNextPlatformCenter = Vector3.Distance(playerPositionXZ, platformPositionXZ);
        return distanceToNextPlatformCenter < platformRadius;
    }
}
