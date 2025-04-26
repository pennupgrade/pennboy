using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CounterScript : MonoBehaviour
//IInteractable
{
    public GameObject player;
    private OrderScript orders;
    public TextMeshProUGUI currOrder;
    public List<string> milkTypes;
    private int amt;
    private string type;
    public int score;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        milkTypes = orders.milkType;
        amt = (orders.GetCurrOrder())[0];
        type = milkTypes[(orders.GetCurrOrder())[1]];
        currOrder.text = "Current Order: \n Milk:" + type + "\n MilkAmt: " + amt;
    }



    /*private void Start() {
         player = GameObject.FindWithTag("Player");
         order = FindObjectOfType<OrderScript>();
     }

     public void Update()
     {

     }
     public void Interact() {
         if (player != null)
         {
             PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
             if (holdItem != null)
             {
                 ItemScript item = holdItem.GetItemHeld();
                 if (item != null) {
                     if (item.IsServable) {
                         return;
                     }
                 }
             }
         }
     }

     public void Serve() {
         return;
     }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            MugScript mug = other.gameObject.GetComponent<MugScript>();
            List<int> contents = mug.contents; 
            List<int> lastOrder = orders.GetCurrOrder(); // save completed order
            AssignPoints(CheckOrder(lastOrder, contents));
            orders.OntoNextOrder(); // dequeue completed order
            amt = (orders.GetCurrOrder())[0]; // amt for next order
            type = milkTypes[(orders.GetCurrOrder())[1]]; // type for next order
            currOrder.text = currOrder.text = "Current Order: \n Milk:" + type + "\n MilkAmt: " + amt; // updated display
            Debug.Log("Mug placed on the counter!");
        }
    }

    private bool CheckOrder(List<int> lastOrder, List<int> contents)
    {
        for (int i = 0; i < 2; i++)
        {
            if (lastOrder[i] != contents[i])
            {
                return false;
            }
        }
        return true;
    }

    private void AssignPoints(bool isOrderCorrect)
    {

    }
}
