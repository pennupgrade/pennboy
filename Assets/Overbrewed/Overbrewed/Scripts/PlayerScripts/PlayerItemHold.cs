using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHold : MonoBehaviour
{
    private GameObject heldItem = null;
    public Transform holdPoint; // Assign an empty GameObject as the hand position

    public bool CanHoldItem()
    {
        return heldItem == null; // Only hold one item at a time
    }

    public void HoldItem(GameObject item)
    {
        if (heldItem == null)
        {
            heldItem = item;
            heldItem.transform.SetParent(holdPoint);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity;
        }
    }

    public ItemScript GetItemHeld() {
        if (heldItem != null)
        {
            ItemScript objectInfo = heldItem.GetComponent<ItemScript>();
            if (objectInfo != null) {
                return objectInfo;
            }
            else
            {
                Debug.LogWarning("Held item does not have an iteminfo component!");
            }
        }
        return null;
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem = null;
        }
    }
}

