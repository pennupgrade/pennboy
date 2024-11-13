using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTint : MonoBehaviour
{
    public Image tintImage; // Drag the ScreenTint Image here in the Inspector
    public float fadeDuration = 1.0f; // Duration for fade in and out

    private void Start()
    {
        if (tintImage != null)
        {
            // Ensure the tint is initially invisible
            Color color = tintImage.color;
            color.a = 0;
            tintImage.color = color;
        }
    }

    public void TintAndFade()
    {
        Debug.Log("Attempting to screen tint");
        if (tintImage != null)
        {
            StartCoroutine(FadeTint());
        }
    }

    private IEnumerator FadeTint()
    {
        // Step 1: Fade In (make the screen green)
        float timer = 0;
        Color color = tintImage.color;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            tintImage.color = color;
            yield return null;
        }

        // Step 2: Pause for a moment (optional)
        yield return new WaitForSeconds(0.5f);

        // Step 3: Fade Out (restore normal color)
        timer = 0;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, timer / fadeDuration);
            tintImage.color = color;
            yield return null;
        }
    }
}