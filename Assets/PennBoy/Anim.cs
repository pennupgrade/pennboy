using System;
using System.Collections;
using UnityEngine;

namespace PennBoy
{
public static class Anim
{
    public static IEnumerator Animate(float animTime, Action<float> enumerate) {
        var elapsedTime = 0f;

        while (elapsedTime <= animTime) {
            var t = elapsedTime / animTime;
            enumerate(t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure animation finishes completely e.g. completely interpolates to 1
        enumerate(1f);
    }
}
}