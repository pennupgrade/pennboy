using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagerScript : MonoBehaviour
{
    public static ItemManagerScript Instance;

    public List<ItemScript> Items;
    public List<RecipeScript> Recipes;

    void Awake()
    {
        Instance = this;
    }
}
