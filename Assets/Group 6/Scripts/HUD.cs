using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Rendering;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text budgetText;
    [SerializeField] private TMP_Text addLossBudgetText;
    [SerializeField] private TMP_Text coinText;
    private List<int> addLossList = new();
    private int maxBudget;
    private int budget;
    private int displayBudget;
    private int maxDeltaDisplayBudgetPerDecrease = 2;
    private float updateBudgetDisplayTime = 0.05f;
    private bool currentFirstElementInAddLossListIsAPlus = true;
    private float timeStore = 0f;
    private int secretCoins;
    // Start is called before the first frame update
    void Start()
    {
        timeStore = Time.time;
        InvokeRepeating("updateBudgetDisplay", 0, updateBudgetDisplayTime);
    }

    // Update is called once per frame
    // void Update()
    // {
    //     timeText.text = "Time: " + $"{Mathf.Floor((Time.time - timeStore) * 100) / 100f}";
    // }

    void Update()
    {
        float elapsedTime = Time.time - timeStore;

        int minutes = Mathf.FloorToInt(elapsedTime / 60); // Calculate the minutes
        int seconds = Mathf.FloorToInt(elapsedTime % 60); // Calculate the remaining seconds
        int centiseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);

        timeText.text = $"Time: {minutes:00}:{seconds:00}.{centiseconds:00}"; // Format as MM:SS
    }

    private void updateBudgetDisplay() {
        if (addLossList.Count > 0) {
            if (addLossList[0] == 0) {
                addLossList.RemoveAt(0);
            } else {
                int initial = addLossList[0];
                int final = (int) Mathf.MoveTowards(initial, 0, maxDeltaDisplayBudgetPerDecrease);
                if (final == 0) {
                    currentFirstElementInAddLossListIsAPlus = initial > 0;
                }
                addLossList[0] = final;
                displayBudget -= final - initial;
                updateDisplayedBudget();
            }
            updateAddLossBudgetText();
        }
    }

    public void reduceBudget(int damage) {
        budget = Mathf.Clamp(budget - damage, 0, maxBudget);
        addLossList.Add(-damage);
    }

    private void updateDisplayedBudget() {
        budgetText.text = $"<align=left><color=black>Total:<line-height=0>\n<align=right>${displayBudget}<line-height=1em>";
    }

    private void updateDisplayedCoins() {
        coinText.text = "Secret Coins: " + $"{secretCoins}";
    }

    // TODO: Optimize later (only process non first elements once....)
    private void updateAddLossBudgetText() {
        String text = "";
        foreach (int num in addLossList) {
            if (num < 0 || (num == 0 && !currentFirstElementInAddLossListIsAPlus)) {
                text += "<color=red>-$" + Math.Abs(num) + "\n";
            } else {
                text += "<color=#029322>+$" + Math.Abs(num) + "\n";
            }
        }
        addLossBudgetText.text = text;
    }

    public void setBudget(int initialBudget, int maxBudget) {
        this.budget = initialBudget;
        this.displayBudget = initialBudget;
        this.maxBudget = maxBudget;
        updateDisplayedBudget();
    }

    public void addCoin() {
        //Debug.Log("Adding coin in HUD");
        this.secretCoins++;
        updateDisplayedCoins();
    }

    public int getBudget() {
        return budget;
    }

    public int getCoins() {
        return secretCoins;
    }

    public float getTime() {
        return Time.time - timeStore;
    }
}
