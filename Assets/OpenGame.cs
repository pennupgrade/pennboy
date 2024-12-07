using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour
{
    public int gameID;

    void onClicked() {
        Debug.Log("Opening Game " + gameID);
        SceneManager.LoadScene(gameID);
    }
}
