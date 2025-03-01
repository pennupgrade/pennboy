using System.Collections;
using System.Collections.Generic;
using PennBoy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenGame : MonoBehaviour
{
    //public int gameID; // temp fixes
    public string gameID;
    public RectTransform targetTransform;

    public ButtonController parent;
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
        GetComponent<Button>().enabled = false;

        parent.disableGameButtons(gameID);
    }

    private IEnumerator GameTransition() {
        RectTransform childTransform = GetComponentsInChildren<RectTransform>()[1];

        var imgTargetTransform = targetTransform.position + new Vector3(0.0f, 60.0f, 0.0f);

        AudioSource audio = GameObject.Find("Cookies").GetComponent<AudioSource>();
        Debug.Log("Audio: " + audio);

        // audio.volume = 0;

        StartCoroutine(Anim.Animate(1.5f, t =>
        {
            GameObject.Find("Cookies").GetComponent<AudioSource>().volume = (1 - t);
        }));

        yield return Anim.Animate(1.0f, t => {
            rectTransform.position = t * targetTransform.position + (1-t) * initRectTransform.position;
            rectTransform.sizeDelta = t * targetTransform.sizeDelta + (1-t) * initRectTransform.sizeDelta;
            // childTransform.position = t * (imgTargetTransform) + (1-t) * initRectTransform.position;
            // game_img.sizeDelta = t * targetTransform.sizeDelta + (1-t) * initRectTransform.sizeDelta;
            var color = (1-t) * Color.white;
            color.a = 1;
            GetComponent<Image>().color = color;
        });

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(gameID);
    }
}
