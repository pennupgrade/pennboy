using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachineScript : MonoBehaviour, IInteractable
{
    public GameObject coffeePrefab; // Assign coffee prefab in Inspector
    public GameObject player;
    private bool isBrewing = false;
    private bool isCoffeeReady = false;

    private void Start() {
        player = GameObject.FindWithTag("Player");
    } 

    public void Interact()
    {
        if (player != null) {
            PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
            if (holdItem != null && holdItem.CanHoldItem()) { // Only allows interaction with coffee machine when hand is empty
                if (isBrewing)
                {
                    Debug.Log("Coffee is still brewing...");
                }
                else if (isCoffeeReady)
                {
                    GiveCoffeeToPlayer();
                }
                else
                {
                    StartCoroutine(BrewCoffee());
                }
            }
        }
    }

    private IEnumerator BrewCoffee()
    {
        isBrewing = true;
        Debug.Log("Brewing coffee...");

        yield return new WaitForSeconds(10); // Simulate brewing time

        isBrewing = false;
        isCoffeeReady = true;
        Debug.Log("Coffee is ready! Pick it up.");
    }

    private void GiveCoffeeToPlayer()
    {
        if (player != null)
        {
            PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
            if (holdItem != null && holdItem.CanHoldItem())
            {
                holdItem.HoldItem(Instantiate(coffeePrefab));
                isCoffeeReady = false; // Reset machine for next brew
                Debug.Log("Player received coffee!");
            }
            else
            {
                Debug.Log("Player's hands are full!");
            }
        }
    }
}