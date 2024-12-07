using UnityEngine;

namespace PennBoy
{
public static class Easing
{
    public static float EaseOutExpo(float t) {
        return Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    }

    public static float EaseInExpo(float t) {
        return Mathf.Approximately(t, 0f) ? 0f : Mathf.Pow(2, 10f * t - 10f);
    }

    public static float EaseInOutQuint(float t) {
        return t < 0.5f ? 16f * t * t * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 5f) / 2f;
    }

    public static float EaseInOutExpo(float t) {
        if (Mathf.Approximately(t, 0f)) return 0f;
        if (Mathf.Approximately(t, 1f)) return 1f;

        return t < 0.5f ? Mathf.Pow(2, 20f * t - 10f) / 2f : (2f - Mathf.Pow(2, -20f * t + 10f)) / 2f;
    }
}
}