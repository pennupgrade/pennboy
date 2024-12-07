using System.Collections;
using System.Collections.Generic;
using PennBoy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenGame : MonoBehaviour
{
    public int gameID;
    public RectTransform targetTransform;
    // public RectTransform game_img;

    private RectTransform rectTransform;

    private RectTransform initRectTransform;
    
    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        initRectTransform = rectTransform;
    }


    public void onClicked() {
        Debug.Log("CLICKED");
        GetComponent<Button>().enabled = false;

        Debug.Log("Opening Game " + gameID);

        StartCoroutine(GameTransition());
    }

    private IEnumerator GameTransition() {
        yield return Anim.Animate(1.0f, t => {
            rectTransform.position = t * targetTransform.position + (1-t) * initRectTransform.position;
            rectTransform.sizeDelta = t * targetTransform.sizeDelta + (1-t) * initRectTransform.sizeDelta;
            // game_img.sizeDelta = t * targetTransform.sizeDelta + (1-t) * initRectTransform.sizeDelta;
        });

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(gameID);
    }
}
