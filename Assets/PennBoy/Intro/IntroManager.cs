using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PennBoy
{
public class IntroManager : MonoBehaviour
{
    [Header("Background")]
    public GameObject repeatingBg;

    [Header("HealthSafety")]
    public GameObject healthSafety;
    public List<GameObject> text;
    public PressAnyKeyBehavior pressAnyKey;

    [Header("Logo Sequence")]
    public GameObject logoSequence;
    public GameObject upgradeLogo;
    public List<GameObject> letters;
    public List<Image> stars;

    private bool polling;
    private (RectTransform rectTransform, Image image) bgAttrs;
    private (RectTransform rectTransform, CanvasGroup canvasGroup) logoAttrs;
    private List<(RectTransform rectTransform, CanvasGroup canvasGroup, Image image)> letterAttrs;
    private List<(RectTransform rectTransform, CanvasGroup canvasGroup)> textAttrs;

    public Image transitionObj;

    private void Awake() {
#if UNITY_EDITOR
        healthSafety.SetActive(true);
        logoSequence.SetActive(false);
#endif
        // Get all necessary components
        bgAttrs = (repeatingBg.GetComponent<RectTransform>(), repeatingBg.GetComponent<Image>());
        logoAttrs = (upgradeLogo.GetComponent<RectTransform>(), upgradeLogo.GetComponent<CanvasGroup>());

        letterAttrs = new List<(RectTransform, CanvasGroup, Image)>(letters.Count);
        foreach (var letter in letters) {
            letterAttrs.Add((letter.GetComponent<RectTransform>(),
                             letter.GetComponent<CanvasGroup>(),
                             letter.GetComponent<Image>()));
        }

        textAttrs = new List<(RectTransform, CanvasGroup)>(text.Count);
        foreach (var obj in text) {
            textAttrs.Add((obj.GetComponent<RectTransform>(), obj.GetComponent<CanvasGroup>()));
        }

        // Set everything to their default states
        pressAnyKey.paused = true;
        bgAttrs.image.color = Color.black;
        textAttrs.ForEach((attrs) => attrs.canvasGroup.alpha = 0f);
        logoAttrs.rectTransform.anchoredPosition = new Vector2(logoAttrs.rectTransform.anchoredPosition.x, -600);
        logoAttrs.canvasGroup.alpha = 1f;

        foreach (var (rectTrans, canvasGroup, image) in letterAttrs) {
            canvasGroup.alpha = 0f;
            rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, -700);
            image.color = Theme.Up[4];
        }
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(0.3f);

        // Zoom background in
        var initScale = bgAttrs.rectTransform.localScale;
        var finalScale = new Vector3(2.5f, 2.5f, 2.5f);
        StartCoroutine(Anim.Animate(0.8f, t => {
            t = Easing.EaseOutExpo(t);
            bgAttrs.rectTransform.localScale = Vector3.Lerp(initScale, finalScale, t);
        }));

        // Fade background color into a dark grey
        var finalBgCol = new Color(0.106f, 0.106f, 0.106f);
        StartCoroutine(Anim.Animate(0.3f, t => {
            t = Easing.EaseOutExpo(t);
            bgAttrs.image.color = Color.Lerp(Color.black, finalBgCol, t);
        }));

        // Animate text coming in
        foreach (var (rectTransform, canvasGroup) in textAttrs) {
            var finalPos = rectTransform.anchoredPosition;
            var initYPos = finalPos.y - 100f;

            StartCoroutine(Anim.Animate(0.7f, t => {
                canvasGroup.alpha = t;
                rectTransform.anchoredPosition =
                    new Vector2(finalPos.x, Mathf.Lerp(initYPos, finalPos.y, Easing.EaseOutExpo(t)));
            }));

            yield return new WaitForSeconds(0.1f);
        }

        polling = true;
        pressAnyKey.paused = false;
    }

    private void Update() {
        if (!polling) return;

        if (Input.anyKey) {
            polling = false;
            pressAnyKey.paused = true;
            pressAnyKey.Flash();

            StartCoroutine(AnimateLogoSequence());
        }
    }

    private IEnumerator AnimateLogoSequence() {
        // Fade out all the text
        yield return Anim.Animate(0.3f, t => {
            textAttrs.ForEach((attrs) => attrs.canvasGroup.alpha = 1f - t);
        });

        // Zoom out background
        var initBgScale = bgAttrs.rectTransform.localScale;
        var finalBgScale = new Vector3(1.5f, 1.5f, 1.5f);
        StartCoroutine(Anim.Animate(0.7f, t => {
            var newT = Easing.EaseInOutQuint(t);
            bgAttrs.rectTransform.localScale = Vector3.Lerp(initBgScale, finalBgScale, newT);
        }));

        // Fade background to Up[0]
        var initBgCol = bgAttrs.image.color;
        StartCoroutine(Anim.Animate(0.3f, t => {
            bgAttrs.image.color = Color.Lerp(initBgCol, Theme.Up[10], t);
        }));

        healthSafety.SetActive(false);
        logoSequence.SetActive(true);

        // Animate PennBoy logo upwards, letter by letter, fading in
        yield return StartCoroutine(RevealPennBoyLetters(letterAttrs));
        yield return new WaitForSeconds(1.5f);

        foreach (var (_, _, image) in letterAttrs) {
            StartCoroutine(PulsePennBoyLetterColor(image));
            yield return new WaitForSeconds(0.1f);
        }

        foreach (var star in stars) {
            StartCoroutine(ScaleStars(star));
            StartCoroutine(RotateStars(star));
            yield return new WaitForSeconds(0.2f);
        }

        // Zoom in background
        initBgScale = bgAttrs.rectTransform.localScale;
        finalBgScale = new Vector3(2.0f, 2.0f, 2.0f);
        StartCoroutine(Anim.Animate(0.45f, t => {
            var newT = Easing.EaseInOutQuint(t);
            bgAttrs.rectTransform.localScale = Vector3.Lerp(initBgScale, finalBgScale, newT);
        }));

        // Fade and translate in UPGRADE logo
        var initLogoPos = logoAttrs.rectTransform.anchoredPosition;
        var finalLogoPos = new Vector2(logoAttrs.rectTransform.anchoredPosition.x, -207);
        StartCoroutine(Anim.Animate(0.7f, t => {
            logoAttrs.rectTransform.anchoredPosition =
                Vector2.Lerp(initLogoPos, finalLogoPos, Easing.EaseOutExpo(t));
        }));

        yield return new WaitForSeconds(3.0f);

        yield return Anim.Animate(1f, t => {
            transitionObj.color = new Color(transitionObj.color.r, transitionObj.color.g, transitionObj.color.b, t);
        });

        StartCoroutine(LoadNextScene());
        
    }

    private IEnumerator LoadNextScene() {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }


    private IEnumerator ScaleStars(Image star) {
        yield return Anim.Animate(0.5f, t => {
            star.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        });

        yield return Anim.Animate(0.5f, t => {
            star.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
        });
    }

    private IEnumerator RotateStars(Image star) {
        for (var i = 1; i <= 360; ++i) {
            star.rectTransform.Rotate(0f, 0f, 1f);
            yield return null;
        }
    }

    private IEnumerator PulsePennBoyLetterColor(Image image) {
        var initColor = image.color;

        yield return Anim.Animate(0.15f, t => {
            image.color = Color.Lerp(initColor, (Color.white + Theme.Up[4]) / 2, t);
        });

        yield return new WaitForSeconds(0.1f);

        yield return Anim.Animate(0.15f, t => {
            image.color = Color.Lerp((Color.white + Theme.Up[4]) / 2, initColor, t);
        });
    }

    private IEnumerator RevealPennBoyLetters(List<(RectTransform, CanvasGroup, Image)> attrs) {
        foreach (var (rectTrans, canvasGroup, image) in attrs) {
            var initPos = rectTrans.anchoredPosition;
            var finalPos = new Vector2(rectTrans.anchoredPosition.x, 78);

            StartCoroutine(Anim.Animate(0.25f, t => {
                canvasGroup.alpha = t;
            }));

            StartCoroutine(Anim.Animate(1.25f, t => {
                rectTrans.anchoredPosition = Vector2.Lerp(initPos, finalPos, Easing.EaseInOutExpo(t));
            }));

            StartCoroutine(AnimatePennBoyLetterColor(image));

            yield return new WaitForSeconds(0.15f);
        }
    }

    private IEnumerator AnimatePennBoyLetterColor(Image image) {
        yield return new WaitForSeconds(0.1f);

        // Colors go from 4 -> 5 -> 6 -> 7 -> 8
        for (var i = 8; i > 4; i--) {
            var idx = i;

            yield return Anim.Animate(0.1f, t => {
                image.color = Color.Lerp(Theme.Up[idx], Theme.Up[idx - 1], t);
            });

            yield return new WaitForSeconds(0.25f);
        }
    }
}
}