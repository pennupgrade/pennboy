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
    private List<int> addLossList = new();
    private int maxBudget;
    private int budget;
    private int displayBudget;
    private int maxDeltaDisplayBudgetPerFrame = 2;
    private bool currentFirstElementInAddLossListIsAPlus = true;
    private float timeStore = 0f;
    // Start is called before the first frame update
    void Start()
    {
        timeStore = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "Time: " + $"{Mathf.Floor((Time.time - timeStore) * 100) / 100f}";
        if (addLossList.Count > 0) {
            if (addLossList[0] == 0) {
                addLossList.RemoveAt(0);
            } else {
                int initial = addLossList[0];
                int final = (int) Mathf.MoveTowards(initial, 0, maxDeltaDisplayBudgetPerFrame);
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
        budgetText.text = "Budget: $" + $"{displayBudget}";
    }

    // TODO: Optimize later (only process non first elements once....)
    private void updateAddLossBudgetText() {
        String text = "";
        foreach (int num in addLossList) {
            if (num < 0 || !currentFirstElementInAddLossListIsAPlus) {
                text += "<color=red>-$" + Math.Abs(num) + "\n";
            } else {
                text += "<color=green>+$" + Math.Abs(num) + "\n";
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

    public int getBudget() {
        return budget;
    }
}
