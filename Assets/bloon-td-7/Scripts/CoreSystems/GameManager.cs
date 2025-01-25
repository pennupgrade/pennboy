using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTD7
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public HealthManager healthManager;
        public MoneyManager moneyManager;
        public WaveManager waveManager;
        public CardManager cardManager;
        public GameObject enemy;

        public GameObject projectile;

        public AudioClip teleportSound;
        public AudioClip explosionSound;
        public AudioClip laserShootSound;


        private void Awake()
        {
            instance = this;
        }

    }
}


