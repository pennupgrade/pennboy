using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderScript : MonoBehaviour
{
    private List<int> orderable;
    public Queue<List<int>> Orders;
    private List<int> currentServed;
    // Start is called before the first frame update
    void Start()
    {
        Orders = new Queue<List<int>>();
        orderable = new List<int>() {2, 3, 4};
        GenerateOrder();
    }

    private void GenerateOrder() {
        
        while(true) {
            int order_size = Random.Range(1, 3);
            List<int> order = new();
            while(order.Count < order_size){
                order.Add(GetRandomItemID());
            }
            Orders.Enqueue(order);
        }
    }
    public int GetRandomItemID() {
        int randomIndex = Random.Range(0, orderable.Count);
        return orderable[randomIndex];
    }
}
