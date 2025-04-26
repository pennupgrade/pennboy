using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{
    public Queue<List<int>> orders;
    public List<string> milkType;
    private List<int> milkAmt;
    private List<int> milkTypeNum;
    private List<int> currentServed;
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
}
