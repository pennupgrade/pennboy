using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    public int[] ingredients;
    public int result;
    public Recipe(int[] ingred, int res) {
        ingredients = ingred;
        result = res;
    }
}
