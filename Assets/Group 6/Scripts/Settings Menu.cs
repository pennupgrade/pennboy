using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    public Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;
    public static float mouseSensitivity = 1;

    private void Start() {
        // Load saved sensitivity from PlayerPrefs or default to 1.0 if not set
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        sensitivitySlider.value = savedSensitivity;
    }

    public void SetMouseSensitivity(float sensitivity) {
        mouseSensitivity = sensitivity;
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity); // Save to PlayerPrefs
        sensitivityValueText.text = "x" + mouseSensitivity.ToString("0.00");
    }
}