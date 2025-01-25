using UnityEngine;

public class Death : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gameManager;
    public bool isDead;
    public SnowboardCharlie2 pmovement;
    public float deathDelay;
    public Grounded playerGrounded;
    public float deathAngle;
    private Rigidbody playerRB;
    private Vector3 deathlaunch;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        deathDelay = 0;
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deathDelay == 1) {
            Die();
        }
    }

    public bool CheckDeath() {
        bool isToDie = false;
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f, playerGrounded.layerMask);
            float landingAngle = Vector3.Angle(hitInfo.normal, -1 * transform.parent.up);
            // if (this.transform.parent.rotation.eulerAngles.x > 180) {
            //     landingAngle = 360 - this.transform.parent.rotation.eulerAngles.x;
            // } else {
            //     landingAngle = this.transform.parent.rotation.eulerAngles.x;
            // }
            isToDie = landingAngle < deathAngle;
            //print(landingAngle + " " + isToDie);
            if (isToDie) {
                deathDelay += 1f;
                playerRB.drag = 0.75f;
                //deathlaunch = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);
                
            }
       
        return isToDie;
    }
    public void Die()
    {   
        pmovement.enabled = false;
        // playerRB.AddForce(Vector3.Normalize(deathlaunch), ForceMode.Impulse);
        // playerRB.AddTorque(transform.up * 15f, ForceMode.Impulse);
        isDead = true;
        gameManager.GameOver();
    }
    void FixedUpdate() {
        if (deathDelay >= 1) {
            deathDelay--;
        }
    }

   
}
