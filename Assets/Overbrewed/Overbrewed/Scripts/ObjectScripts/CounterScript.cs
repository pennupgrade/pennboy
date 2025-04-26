using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CounterScript : MonoBehaviour
//IInteractable
{
    public GameObject player;
    public OrderScript orders;
    public TextMeshProUGUI currOrder;
    public List<string> milkTypes;
    private int amt;
    private string type;
    public int score;
    public TextMeshProUGUI scoreDisplay;
    private void Start()
    {
        orders = FindObjectOfType<OrderScript>();
        player = GameObject.FindWithTag("Player");

        milkTypes = orders.milkType;

        Debug.Log("Next Order: Amt = " + amt + ", Type Index = " + (orders.GetCurrOrder())[1]);

        amt = (orders.GetCurrOrder())[0];
        type = milkTypes[(orders.GetCurrOrder())[1]];

        scoreDisplay.text = "Score: 0";
        currOrder.text = "Current Order: \n Milk:" + type + "\n MilkAmt: " + amt;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mug"))
        {
            Debug.Log("triggered");
            MugScript mug = other.gameObject.GetComponent<MugScript>();
            mug.ChangeMugColor(Color.green);
            List<int> contents = mug.contents;

            List<int> lastOrder = orders.GetCurrOrder(); 

            AssignPoints(CheckOrder(lastOrder, contents));

            orders.OntoNextOrder(); 
            amt = (orders.GetCurrOrder())[0]; 
            type = milkTypes[(orders.GetCurrOrder())[1]]; 
            currOrder.text = "Current Order: \n Milk:" + type + "\n MilkAmt: " + amt; 

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
        Debug.Log("points assigned");
        if (isOrderCorrect)
        {
            score += 10;
        }
        else
        {
            score += 2;
        }
        scoreDisplay.text = "Score: " + score.ToString();
    }
}
