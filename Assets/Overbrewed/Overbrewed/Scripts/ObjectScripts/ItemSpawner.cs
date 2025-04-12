using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public string prefabFolderPath = "Prefabs";  // Path to prefabs in Resources folder (relative to "Assets/Resources/")
    private Dictionary<int, GameObject> prefabDictionary = new();
    public List<int> Servable = new();

    void Start()
    {
        LoadAllPrefabs();
    }

    // Load all prefabs from Resources/Prefabs folder
    void LoadAllPrefabs()
    {
        // Load all prefabs in the specified folder
        GameObject[] prefabs = Resources.LoadAll<GameObject>(prefabFolderPath);
        
        // Iterate through each prefab and store relevant data
        foreach (var prefab in prefabs)
        {
            // Check if prefab has ItemData (or any component storing data)
            var item = prefab.GetComponent<ItemScript>();
            if (item != null)
            {
                // Store prefab in dictionary with ID as key
                prefabDictionary[item.ItemID] = prefab;
                if (item.IsServable) {
                    Servable.Add(item.ItemID);
                }
            }
            else
            {
                Debug.LogWarning($"Prefab {prefab.name} does not have a valid ItemScript component.");
            }
        }
    }

    // Example of spawning an item at a given position
    public void SpawnItem(int itemID)
    {
        if (prefabDictionary.TryGetValue(itemID, out GameObject prefab))
        {
            Instantiate(prefab);
        }
        else
        {
            Debug.LogError($"No prefab found for item ID {itemID}");
        }
    }
}
