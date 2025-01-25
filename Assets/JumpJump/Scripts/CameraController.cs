using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _posOffset; // Camera's position relative to the platform it is centered on
    private float duration = 0.3f;

    private void Start()
    {
        _posOffset = transform.position;
    }

    /// <summary>
    /// Smoothly pans the camera from its current position to a specified destination over a given duration.
    /// The camera movement follows a cubic easing function for a smooth transition.
    /// </summary>
    /// <param name="dest">The target destination position for the camera.</param>
    /// <param name="duration">The duration of the camera pan in seconds.</param>
    /// <param name="onMoveCameraCompleted">Callback function that gets called when the camera movement is complete.</param>
    /// <returns>An IEnumerator used for the coroutine execution of the camera pan.</returns>
    IEnumerator SmoothPan(Vector3 dest, float duration, OnMoveCameraCompleted onMoveCameraCompleted)
    {
        Vector3 start = transform.position;
        float time = 0;
        while (time < duration)
        {
            float u = time / duration;
            transform.position = Vector3.Lerp(start, dest, CubiceaseInOutCubicBezier(u));
            time += Time.deltaTime;
            yield return null;
        }

        onMoveCameraCompleted();
    }

    private float CubiceaseInOutCubicBezier(float u)
    {
        return u < 0.5 ? 4 * u * u * u : 1 - Mathf.Pow(-2 * u + 2, 3) / 2;
    }
    
    
    public delegate void OnMoveCameraCompleted();
    /// <summary>
    /// Moves the camera smoothly to a position offset from the current platform's position.
    /// The movement is done over a set duration using a smooth pan effect.
    /// </summary>
    /// <param name="currentPlatformPos">The position of the platform that the camera should focus on.</param>
    /// <param name="onMoveCameraCompleted">Callback function that gets invoked when the camera movement is complete.</param>
    public void MoveCamera(Vector3 currentPlatformPos, OnMoveCameraCompleted onMoveCameraCompleted)
    {
        StartCoroutine(SmoothPan(currentPlatformPos + _posOffset, duration, onMoveCameraCompleted));
    }
}
