using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseUI;
    private float timeScale;
    private CursorLockMode prevLockState;
    private bool isPaused;
    private static Pause instance = null;
    public string[] lockedScenes;

    public void TogglePauseGame()
    {
        // not paused
        timeScale = (Time.timeScale != 0f) ? Time.timeScale : timeScale;
        prevLockState = (Cursor.lockState == CursorLockMode.None) ? prevLockState : Cursor.lockState;

        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : prevLockState;
        Cursor.visible = (Cursor.lockState == CursorLockMode.None);

        Time.timeScale = isPaused ? 0f : timeScale; // true means is paused now
        Debug.Log("Timescale is now " + Time.timeScale);
        Debug.Log("LockState: " + Cursor.lockState);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume hit");
        isPaused = true;
        TogglePauseGame();
    }

    public void ReturnHome()
    {
        Cursor.lockState = CursorLockMode.None;
        prevLockState = CursorLockMode.None;
        ResumeGame();
        SceneManager.LoadScene("HomePage");
    }

    bool CheckIfBad()
    {
        //  This can be optimized if we cache by current scene name, but since we have so few locked scenes it doesn't matter
        string sceneName = SceneManager.GetActiveScene().name;

        foreach (var locked in lockedScenes)
        {
            if (locked == sceneName)
            {
                return true;
            }
        }

        return false;
    }

    void Awake()
    {
        if (instance == null)
        {
            // default behavior: pause = isPaused
            instance = this;
            timeScale = Time.timeScale;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CheckIfBad())
        {
            TogglePauseGame();
        }
    }
}