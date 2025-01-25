using UnityEngine;

public class Grounded : MonoBehaviour
{
    public bool IsGrounded;

    public LayerMask layerMask;

    public SnowboardCharlie2 playerMovement;
    public CameraMovement cameraMovement;
    public Death death;

    private void OnTriggerEnter(Collider other)
    {   
        if (!IsGrounded) {death.CheckDeath();}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (layerMask == (layerMask | (1 << other.gameObject.layer)))
    //    {

    //        IsGrounded = false;
    //        playerMovement.CurrentState = PlayerMovement.PlayerState.Midair;
    //        cameraMovement.CurrentState = CameraMovement.CameraState.Midair;

    //    }
    //}

    private void Update()
    {
        if (!death.isDead) {
            if (Physics.Raycast(transform.parent.position, Vector3.down, 1.75f, layerMask))
            {
                if (!IsGrounded)
                {
                    print("Landing Check");
                    if (death.CheckDeath()) {
                        IsGrounded = false; 
                        //playerMovement.enabled = false;
                    } else { 
                        IsGrounded = true;
                        playerMovement.CurrentState = SnowboardCharlie2.PlayerState.Grounded;
                        cameraMovement.CurrentState = CameraMovement.CameraState.Grounded; 
                    }
                    
                }
            }
            else
            {
                if (IsGrounded)
                {
                    IsGrounded = false;
                    playerMovement.CurrentState = SnowboardCharlie2.PlayerState.Midair;
                    playerMovement.airTimeStart = Time.time;
                    cameraMovement.CurrentState = CameraMovement.CameraState.Midair;
                } 
            }
        }

    }

}
