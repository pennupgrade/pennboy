using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderScript : MonoBehaviour
{
    public List<List<int>> orders;
    public List<string> milkType;
    private List<int> milkAmt;
    private List<int> milkTypeNum;
    private List<int> currentServed;
    public ScoreScript score;
    // Start is called before the first frame update
    void Awake()
    {
        orders = new List<List<int>>();
        milkAmt = new List<int>() {0, 1, 2, 3};
        milkTypeNum = new List<int>() { 0, 1, 2 };
        milkType = new List<string>() { "whole", "almond", "oat"}; 
        GenerateOrder();
    }

    private void GenerateOrder() {
        for (int i = 0; i < 30; i++)
        {
            List<int> order = new();
            order.Add(GetRandomItemID().Item1);
            order.Add(GetRandomItemID().Item2);
            orders.Add(order);
        }
    }
    public (int,int) GetRandomItemID() {
        int randomIndex1 = Random.Range(0, milkAmt.Count);
        int randomIndex2 = Random.Range(0, milkTypeNum.Count);
        return (milkAmt[randomIndex1], milkTypeNum[randomIndex2]);
    }

    public List<int> GetCurrOrder()
    {
        return orders[0];
    }

    public void OntoNextOrder()
    {
        orders.RemoveAt(0);
    }
    public string GetMilk(int index)
    {
        return milkType[index];
    }

    public List<List<int>> getOrders()
    {
        return orders;
    }
}
