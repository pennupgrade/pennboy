using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiscript : MonoBehaviour
{
    public TextMeshProUGUI currOrder;
    public void updateText(string type, int amt)
    {
        currOrder.text = "Current Order: \n Milk:" + type + "\n MilkAmt: " + amt;
    }
}
