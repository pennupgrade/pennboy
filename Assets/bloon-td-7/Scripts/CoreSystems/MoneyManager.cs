using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal.Internal;

public class MoneyManager : MonoBehaviour
{
    //testing money
    private int startingMoney = 1000; 
    private int currentMoney;

    [SerializeField] public GameObject moneyUI; 
    private TextMeshProUGUI moneyText;
    private Vector3 origin;
    private RectTransform recttransform;
    private int PreUpdateVal;

    void Start()
    {
        recttransform = moneyUI.GetComponent<RectTransform>();
        origin = recttransform.position;
        moneyText = moneyUI.GetComponent<TextMeshProUGUI>();
        currentMoney = startingMoney; 
        PreUpdateVal = currentMoney;
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

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    void UpdateMoneyUI()
    {
        if (PreUpdateVal < currentMoney)
        {
            recttransform.position = origin + new Vector3(0, 20, 0);
            moneyText.color = new Color(255, 255, 100);
        } else
        {
            recttransform.position = origin - new Vector3(0, 10, 0);
            moneyText.color = new Color(255, 0, 0);
        }
        PreUpdateVal = currentMoney;
        moneyText.text = currentMoney.ToString(); //"Money: $" + currentMoney.ToString(); 
    }

    protected void Update()
    {
        // moving schenanigains :)))))))))))
        if ((recttransform.position - origin).magnitude > .1)
        {
            Vector3 moveamt = Vector3.Lerp(recttransform.position, origin, 10f * Time.deltaTime);
            recttransform.position = moveamt;

            float gval = Mathf.Lerp(moneyText.color.g, 217, 5f * Time.deltaTime);
            float bval = Mathf.Lerp(moneyText.color.b, 0, 5f * Time.deltaTime);
            moneyText.color = new Color(255, gval, bval);
        }
    }
}
