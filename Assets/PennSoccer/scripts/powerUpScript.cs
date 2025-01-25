using System.Collections;
using UnityEngine;

public class powerUpScript: MonoBehaviour
{
    public float effectDuration = 3.0f;
    GameObject player1obj;
    GameObject player2obj;
    GameObject powerupDisplay;

    public Sprite speedUp;
    public Sprite jumpUp;
    public Sprite sizeUp;


    private void Start()
    {
        player1obj = GameObject.Find("player 1");
        player2obj = GameObject.Find("player 2");
        powerupDisplay = GameObject.Find("powerUpDisplay");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PennSoccerPlayer"))
        {
            player1movement player1 = other.GetComponent<player1movement>();
            player2movement player2 = other.GetComponent<player2movement>();

            if(player1 != null)
            {
                ApplyPowerUp(player1obj);
            }

            else if (player2 != null)
            {
                ApplyPowerUp(player2obj);
            }
        }
    }

    private void ApplyPowerUp(GameObject player)
    {
        if (gameObject.CompareTag("PennSoccerSuperSpeed"))
        {
            StartCoroutine(ApplySpeedBoost(player));
        }
        else if (gameObject.CompareTag("PennSoccerSuperSize"))
        {
            StartCoroutine(ApplySizeBoost(player));
        }
        else if (gameObject.CompareTag("PennSoccerSuperJump"))
        {
            StartCoroutine(ApplyJumpBoost(player));
        }
    }

    private IEnumerator ApplySpeedBoost(GameObject player)
    {
        
        if (player == player1obj)
        {
            player1obj.GetComponent<player1movement>().speed *= 2;
        }
        else if (player == player2obj)
        {
            player2obj.GetComponent<player2movement>().speed *= 2;
        }
        
        gameObject.transform.position = new Vector3(-17.7f, -0.87f, -7.032f);
        powerupDisplay.GetComponent<SpriteRenderer>().sprite = speedUp;
        //gameObject.SetActive(false);

        yield return new WaitForSeconds(effectDuration);

        if (player == player1obj)
        {
            player1obj.GetComponent<player1movement>().speed /= 2;
        }
        else if (player == player2obj)
        {
            player2obj.GetComponent<player2movement>().speed /= 2;
        }
        Destroy(gameObject);
   
    }

    private IEnumerator ApplyJumpBoost(GameObject player)
    {

        if (player == player1obj)
        {
            player1obj.GetComponent<player1movement>().jumpForce *= 1.5f;
        }
        else if (player == player2obj)
        {
            player2obj.GetComponent<player2movement>().jumpForce *= 1.5f;
        }

        gameObject.transform.position = new Vector3(-17.7f, -0.87f, -7.032f);
        powerupDisplay.GetComponent<SpriteRenderer>().sprite = jumpUp;
        //gameObject.SetActive(false);

        yield return new WaitForSeconds(effectDuration);

        if (player == player1obj)
        {
            player1obj.GetComponent<player1movement>().jumpForce /= 1.5f;
        }
        else if (player == player2obj)
        {
            player2obj.GetComponent<player2movement>().jumpForce /= 1.5f;
        }
        Destroy(gameObject);

    }

    private IEnumerator ApplySizeBoost(GameObject player)
    {
        if (player == player1obj || player == player2obj) {

            Vector3 originalSize = player.transform.localScale;

            player.transform.localScale = originalSize * 2;

            gameObject.transform.position = new Vector3(-17.7f, -0.87f, -7.032f);
            powerupDisplay.GetComponent<SpriteRenderer>().sprite = sizeUp;

            //gameObject.SetActive(false);

            yield return new WaitForSeconds(effectDuration);

            player.transform.localScale = originalSize;

            Destroy(gameObject);
        }
    }
}