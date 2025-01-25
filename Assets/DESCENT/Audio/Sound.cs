using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;

    public AudioClip audioClip;

    public float volume = 0.5f;
    [Range (0.1f, 3f)]
    public float pitch = 1f;

    [Range(0f, 1f)]
    public float globalVol = 0.5f;

    public bool isLooping;

    public bool playOnAwake;

    [HideInInspector()]
    public AudioSource audioSource;

}
