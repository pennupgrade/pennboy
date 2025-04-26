using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderScript : MonoBehaviour
{
    public Queue<List<int>> orders;
    public List<string> milkType;
    private List<int> milkAmt;
    private List<int> milkTypeNum;
    private List<int> currentServed;
    public float timeRemaining = 30f;
    public List<int> item;
    public bool isActive = true;
    public Text timer;
    public OrderGenerationScript orderGenerator;
    public ScoreScript score;
    // Start is called before the first frame update
    void Start()
    {
        orders = new Queue<List<int>>();
        milkAmt = new List<int>() {0, 1, 2, 3};
        milkTypeNum = new List<int>() { 0, 1, 2 };
        milkType = new List<string>() { "whole", "almond", "oat"}; 
        GenerateOrder();
    }

    private void GenerateOrder() {
        Debug.Log("triggered");
        for (int i = 0; i < 30; i++)
        {
            List<int> order = new();
            order.Add(GetRandomItemID().Item1);
            order.Add(GetRandomItemID().Item2);
            orders.Enqueue(order);
        }
    }
    public (int,int) GetRandomItemID() {
        int randomIndex1 = Random.Range(0, milkAmt.Count);
        int randomIndex2 = Random.Range(0, milkTypeNum.Count);
        return (milkAmt[randomIndex1], milkTypeNum[randomIndex2]);
    }

    public List<int> GetCurrOrder()
    {
        return orders.Peek();
    }

    public void OntoNextOrder()
    {
        orders.Dequeue();
    }
    public string GetMilk(int index)
    {
        return milkType[index];
    }

    public Queue<List<int>> getOrders()
    {
        return orders;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timeRemaining -= Time.deltaTime; // Decrease time

            // Update UI timer display
            if (timer != null)
            {
                timer.text = Mathf.Ceil(timeRemaining).ToString() + "s";
            }

            // If time runs out, remove the order
            if (timeRemaining <= 0)
            {
                ExpireOrder();
            }
        }
    }

    void ExpireOrder()
    {
        isActive = false;
        orderGenerator.RemoveOrder(this);
        score.updateScore(-50);
        Destroy(gameObject);
    }
}
