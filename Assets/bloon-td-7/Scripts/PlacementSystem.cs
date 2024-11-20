using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private TowerCard cardUsing;
    private PlacementMode currentMode;
    private List<Tower> towersToSacrifice;

    private void Start()
    {
        mouseIndicator.SetActive(false);
        currentMode = PlacementMode.Selection;
    }

    private void Update()
    {
        // Toggles sacrificing mode when Q pressed and card can sacrifice
        if (currentMode != PlacementMode.Selection && cardUsing.canSacrifice && Input.GetKeyDown(KeyCode.Q))
        {
            if (currentMode == PlacementMode.PlacingTower)
                currentMode = PlacementMode.Sacrificing;
            else
                currentMode = PlacementMode.PlacingTower;
        }

        switch (currentMode)
        {
            case PlacementMode.Sacrificing:
                // Logic for tower upgrading (sacrificing)
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Clicked");
                    Tower towerClicked = inputManager.GetTowerClicked();
                    if (towerClicked != null)
                    {
                        Debug.Log("Tower Clicked");
                        bool addTower = towerClicked.ToggleSacrifice();
                        if (addTower)
                            towersToSacrifice.Add(towerClicked);
                        else
                            towersToSacrifice.Remove(towerClicked);
                    }
                }
                return;
            case PlacementMode.PlacingTower:
                // Logic for tower placement
                (Vector3 MousePosition, bool validplacement) = inputManager.GetPlacementPosition();

                mouseIndicator.transform.position = MousePosition + new Vector3(0, 3.5f, 0);
                if (!validplacement) { return; }

                if (Input.GetMouseButtonDown(0))
                {
                    MoneyManager moneyManager = GameManager.instance.moneyManager;
                    if (moneyManager.GetCurrentMoney() < cardUsing.GetCost()) { return; }

                    // deduct money
                    moneyManager.SpendMoney(cardUsing.GetCost());

                    // this goes hard
                    Tower t = Instantiate(cardUsing.towerPrefab, MousePosition + new Vector3(0, 3.5f, 0), new Quaternion()).GetComponent<Tower>();
                    if (cardUsing.canSacrifice)
                    {
                        int upgradeAmount = calculateTotalSacrifice();
                        t.CalcLevel(upgradeAmount);
                    }

                    disableTowerPlacement(true);
                }
                return;
            case PlacementMode.Selection:
                // Do logic for selecting towers to open a UI menu here
                return;
        }
    }

    /// <summary>
    /// Calculates the total cost of all towers being sacrificed. Only to be called when sacrificing towers, so destroys all towers in the process.
    /// </summary>
    /// <returns></returns>
    private int calculateTotalSacrifice()
    {
        int sum = 0;
        while (towersToSacrifice.Count > 0)
        {
            Tower t = towersToSacrifice[0];
            towersToSacrifice.RemoveAt(0);
            sum += t.GetCost();
            t.Die();
        }
        return sum;
    }

    /// <summary>
    /// Enables tower placement for the specified tower
    /// </summary>
    /// <param name="id">id of tower to place</param>
    /// <param name="card">reference to card calling this function</param>
    public void enableTowerPlacement(TowerCard card)
    {
        if (currentMode != PlacementMode.Selection)
        {
            cardUsing.Deactivate(false);
        }
        cardUsing = card;
        mouseIndicator.SetActive(true);
        currentMode = PlacementMode.PlacingTower;
        if (towersToSacrifice != null)
        {
            foreach (Tower t in towersToSacrifice)
                t.ToggleSacrifice();
        }
        towersToSacrifice = new List<Tower>();
    }

    /// <summary>
    /// Disables tower placement.
    /// </summary>
    /// <param name="didPlace">True if did place the tower, false if exiting on escape</param>
    public void disableTowerPlacement(bool didPlace)
    {
        cardUsing.Deactivate(didPlace);
        mouseIndicator.SetActive(false);
        currentMode = PlacementMode.Selection;
    }

    private enum PlacementMode
    {
        Sacrificing,
        PlacingTower,
        Selection
    }
}
