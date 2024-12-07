using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PennBoy
{
public class PressAnyKeyBehavior : MonoBehaviour
{
    [SerializeField] private CanvasGroup pressAny;
    [SerializeField] private Image keyBorder;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color flashColor;

    public bool paused;

    private float elapsed;

    // Update is called once per frame
    private void Update() {
        if (paused) return;

        pressAny.alpha = 0.4f * Mathf.Sin(elapsed * 5f) + 0.6f;
        elapsed += Time.deltaTime;
    }

    public void Flash() {
        keyBorder.color = flashColor;
        text.color = flashColor;
    }
}
}