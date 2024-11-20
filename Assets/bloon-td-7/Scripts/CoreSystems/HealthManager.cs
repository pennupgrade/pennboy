using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    private int maxHealth = 100; 
    private int currentHealth; 
    [SerializeField] private TextMeshProUGUI healthText; 

    void Start()
    {
        currentHealth = maxHealth; 
        UpdateHealthUI(); 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        if (currentHealth <= 0)
        {
           // currentHealth = 0;
            GameOver(); 
        }
        UpdateHealthUI(); 
    }

    void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth.ToString(); 
    }

    void GameOver()
    {
        Debug.Log("You LOST");
    }
}
