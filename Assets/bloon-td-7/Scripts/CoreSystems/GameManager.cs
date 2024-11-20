using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HealthManager healthManager;
    public MoneyManager moneyManager;
    public WaveManager waveManager;
    public CardManager cardManager;
    public GameObject enemy;
    
    public GameObject projectile;

    private void Awake()
    {
        instance = this;
    }

}
