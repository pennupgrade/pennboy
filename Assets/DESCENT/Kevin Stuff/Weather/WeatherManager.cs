using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public WeatherState startState;
    public float distanceBeforeSpawning;

    public Vector3 spawnOffset = new Vector3(0f, 15f, 100f); 

    public GameObject snowParticlePrefab;
    public GameObject sandParticlePrefab;
    public Transform playerTransform;

    
    public WeatherState CurrentWeather
    {
        get => m_CurrentState;
        set
        {
            switch (value)
            {
                case WeatherState.Snow:
                    OnSnow();
                    m_UpdateHandler = SnowBehavior;
                    break;
                case WeatherState.Sandstorm:
                    OnSand();
                    m_UpdateHandler = SandBehavior;
                    break;
                default:
                    break;
            }

            m_CurrentState = value;
        }
    }
    private WeatherState m_CurrentState;

    private delegate void UpdateHandler();
    private UpdateHandler m_UpdateHandler;

    public enum WeatherState
    {
        Snow,
        Sandstorm
    }


    Vector3 lastPos;

    private void Start()
    {
        CurrentWeather = startState;
    }


    #region Snow
    void OnSnow()
    {
        lastPos = playerTransform.position;
    }

    void SnowBehavior()
    {
        if ((playerTransform.position - lastPos).sqrMagnitude > distanceBeforeSpawning * distanceBeforeSpawning)
        {
            GameObject newSnow = Instantiate(snowParticlePrefab, playerTransform.position + spawnOffset, Quaternion.identity);
            newSnow.GetComponent<DestroyWhenFarFromTarget>().target = playerTransform;
            lastPos = playerTransform.position;
        }
    }
    #endregion

    #region Sand
    void OnSand()
    {

    }

    void SandBehavior()
    {
        if ((playerTransform.position - lastPos).sqrMagnitude > distanceBeforeSpawning * distanceBeforeSpawning)
        {
            GameObject newSnow = Instantiate(sandParticlePrefab, playerTransform.position + spawnOffset, Quaternion.identity);
            newSnow.GetComponent<DestroyWhenFarFromTarget>().target = playerTransform;
            lastPos = playerTransform.position;
        }
    }
    #endregion


    // Update is called once per frame
    void Update()
    {
        m_UpdateHandler?.Invoke();
    }
}
