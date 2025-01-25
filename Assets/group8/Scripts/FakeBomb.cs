using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBomb : MonoBehaviour
{
    /* 
     * bomb of same level can combine
     * Should create new bomb in its existant place
     * */

    // Constants
    public GameObject basicBomb;
    public GameObject fleshBomb;
    public GameObject magmaBomb;
    public GameObject FireBombVFX;

    // changeable variables
    public int level = 0;
    public float newBombPositionParameter = 0.5f; // must be between 0 and 1

    // private variable
    public int parentsLevel = -1; // -1 means had no parent bomb

    // Reference to the new material to apply
    public Material[] level2Color;
    public Material[] magmaMaterial;
    private int n_material;

    // Start is called before the first frame update
    void Start()
    {
        // set color
        n_material = level2Color.Length;
        SetMaterial(level2Color[Mathf.Min(level, n_material)]);
    }


    // Function to change the object's material
    public void SetMaterial(Material material)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            if (renderer.materials.Length == 1)
                renderer.material = material;
            else if (renderer.materials.Length == 2)
                renderer.materials = magmaMaterial;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on this GameObject.");
        }
    }


}
