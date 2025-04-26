using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderGenerationScript : MonoBehaviour
{
    // private List<int> orderable;
    // public List<OrderScript> Orders;
    // private List<int> currentServed;
    // public TimerScript timer;
    // public ScoreScript score;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     score = FindObjectOfType<ScoreScript>();
    //     Orders = new List<OrderScript>();
    //     orderable = new List<int>() {2, 3, 4};
    //     timer = FindObjectOfType<TimerScript>();
    //     GenerateOrder();
    // }

    // private void GenerateOrder() {
        
    //     while(true) {
    //         int order_size = Random.Range(1, 3);
    //         OrderScript order = new();
    //         order.orderGenerator = this;
    //         order.score = score;
    //         order.item = new();
    //         while(order.item.Count < order_size){
    //             order.item.Add(GetRandomItemID());
    //         }
    //         order.item.Sort();
    //         Orders.Add(order);
    //     }
    // }
    // public int GetRandomItemID() {
    //     int randomIndex = Random.Range(0, orderable.Count);
    //     return orderable[randomIndex];
    // }

    // public void ServeOrder(int ItemID) 
    // {
    //     currentServed.Add(ItemID);
    //     currentServed.Sort();
    //     foreach (OrderScript order in Orders) {
    //         if (currentServed.SequenceEqual(order.item)) 
    //         {
    //             // Score +
    //             score.updateScore(currentServed.Count*50);
    //             Orders.Remove(order);
    //         }
    //     }
    // }

    // public void RemoveOrder(OrderScript order) {
    //     Orders.Remove(order);
    // }
}
