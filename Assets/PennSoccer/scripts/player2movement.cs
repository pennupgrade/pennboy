using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player2movement : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private float horizontalInput;
    private Rigidbody player2Rb;
    public bool isGrounded = true;
    public bool facingRight;
    private SpriteRenderer charRender2;

    void Start()
    {
        player2Rb = GetComponent<Rigidbody>();
        charRender2 = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        // Horizontal movement using wasd
        horizontalInput = Input.GetAxis("Horizontal2");
        player2Rb.velocity = new Vector3(Time.fixedDeltaTime * speed * horizontalInput*70, player2Rb.velocity.y, player2Rb.velocity.z);
        
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            player2Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        Flip();
    }

    void Flip()
    {
        if (horizontalInput > 0)
        {
            facingRight = false;
            charRender2.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            facingRight = true;
            charRender2.flipX = true;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PennSoccerGround"))
        {
            isGrounded = true;
        }
        if(collision.gameObject.CompareTag("PennSoccerPlayer") && collision.transform.position.y + 1.43 <= transform.position.y) 
        {
            isGrounded = true;
        }
    }
}
