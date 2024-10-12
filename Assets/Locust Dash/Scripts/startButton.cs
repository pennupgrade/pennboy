using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class startButton : MonoBehaviour
{
    public void onClick() {
        SceneManager.LoadScene(2);
    }
   
}
