using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    private int startingMoney = 100; 
    private int currentMoney;

    [SerializeField] public TextMeshProUGUI moneyText; 

    void Start()
    {
        currentMoney = startingMoney; 
        UpdateMoneyUI();
    }

    public void EarnMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI(); 
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount; 
            UpdateMoneyUI(); 
            return true; 
        }
        return false;
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + currentMoney.ToString(); 
    }
}