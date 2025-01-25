using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerCard : Card
{
    public GameObject towerPrefab;
    // Temporary until sprites
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform recttransform;
    private Vector3 origin;
    private Vector3 Targetpos;
    private int cost;

    private void Start()
    {
        recttransform = GetComponent<RectTransform>();
        origin = recttransform.position;
        Targetpos = origin;
    }

    public override void ResetCard (GameObject towerPrefab, int id)
    {
        this.towerPrefab = towerPrefab;
        this.id = id;
        isUsing = false;
        canSacrifice = true;
        Targetpos = origin;
    }

    public override bool Use()
    {
        Debug.Log("scafa");
        if (isUsing)
        {
            Targetpos = origin;
            BTD7.GameManager.instance.cardManager.placementSystem.disableTowerPlacement(false);
            isUsing = false;
            return false;
        }

        Targetpos = origin + new Vector3(0, 30, 0);
        BTD7.GameManager.instance.cardManager.placementSystem.enableTowerPlacement(this);
        return true;
    }

    /// <summary>
    /// Deactivates the current tower card, either when card was used to place a tower of when card is turned off.
    /// </summary>
    /// <param name="didPlace">True if was placed, false if not placed.</param>
    public override void Deactivate(bool didPlace)
    {
        Targetpos = origin;
        base.Deactivate(didPlace);
        if (!didPlace)
        {
            Debug.Log("Deactivated " + id);
            isUsing = false;
            return;
        }
        BTD7.GameManager.instance.cardManager.removeCardFromHand(this);
    }

    protected override void Update()
    {

        // moving schenanigains :)))))))))))
        if ((recttransform.position - Targetpos).magnitude > .1)
        {
            Vector3 moveamt = Vector3.Lerp(recttransform.position, Targetpos, 10f * Time.deltaTime);
            recttransform.position = moveamt;
        }

        // veer's actual card stuff
        base.Update();
        if (isUsing)
        {
            text.text = id + " USING";
        }
        else
            text.text = id+"";
    }

    public int GetCost()
    {
        return towerPrefab.GetComponent<Tower>().GetCost();
    }
}
