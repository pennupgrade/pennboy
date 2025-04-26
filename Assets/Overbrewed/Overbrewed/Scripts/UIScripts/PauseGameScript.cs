using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameScript : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false); // Initially hide the pause menu
        resumeButton.onClick.AddListener(Resume);
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            isPaused = true;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }
}
