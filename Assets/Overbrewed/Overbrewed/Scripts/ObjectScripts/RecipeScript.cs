using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class RecipeScript : ScriptableObject
{
    public int[] ingredients;
    public int result;
    public RecipeScript(int[] ingred, int res) {
        ingredients = ingred;
        result = res;
    }
}
