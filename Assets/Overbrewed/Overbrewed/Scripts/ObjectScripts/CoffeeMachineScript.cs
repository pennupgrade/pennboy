using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachineScript : MonoBehaviour
{
    public GameObject player;
    public Slider progressBar;
    private bool isBrewing = false;
    private bool isCoffeeReady = false;
    private float brewingTime = 5f;
    private float brewingProgress = 0f;
    private bool isInRange = false;
    private bool isKeyHeld = false;

    public MugScript currentMug = null;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // player is in range & holding mug, coffee isn't already brewing
        if (isInRange && !isBrewing && !isCoffeeReady) 
        {
            if (Input.GetKey(KeyCode.B)) 
            {
                StartBrewing();
                isKeyHeld = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.B) && isKeyHeld)
        {
            StopBrewing();
            isKeyHeld = false;
        }
    }

    public void StartBrewing()
    {
        isBrewing = true;
        brewingProgress = 0f;
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f;
        StartCoroutine(BrewCoffee());
    }

    private void StopBrewing()
    {
        isBrewing = false;
        brewingProgress = 0f;
        progressBar.gameObject.SetActive(false);
    }

    private IEnumerator BrewCoffee()
    {
        while (brewingProgress < brewingTime)
        {
            brewingProgress += Time.deltaTime;
            progressBar.value = brewingProgress;
            yield return null;
        }

        isCoffeeReady = true;
        isBrewing = false;
        currentMug.ChangeMugColor(Color.red);
        progressBar.gameObject.SetActive(false);
        Debug.Log("Coffee is ready! Pick it up.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
        }
        currentMug = player.GetComponentInChildren<MugScript>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}