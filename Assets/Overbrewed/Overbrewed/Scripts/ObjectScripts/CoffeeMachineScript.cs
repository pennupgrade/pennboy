using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachineScript : MonoBehaviour, IInteractable
{
    public GameObject player;
    public Slider progressBar;
    private bool isBrewing = false;
    private bool isCoffeeReady = false;
    private float brewingTime = 5f;
    private float brewingProgress = 0f;
    private bool isInRange = false;
    private bool isKeyHeld = false;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        if (progressBar != null)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
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

        isBrewing = false;
        isCoffeeReady = true;
        GiveCoffeeToPlayer();
        progressBar.gameObject.SetActive(false);
        Debug.Log("Coffee is ready! Pick it up.");
    }


    private void GiveCoffeeToPlayer()
    {
        if (player != null)
        {
            PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
            if (holdItem != null && holdItem.CanHoldItem())
            {
                isCoffeeReady = false;
                Debug.Log("Player received coffee!");
            }
            else
            {
                Debug.Log("Player's hands are full!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}