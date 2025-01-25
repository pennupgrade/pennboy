using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            AudioSource newSound = gameObject.AddComponent<AudioSource>();

            newSound.clip = s.audioClip;

            newSound.volume = s.globalVol;
            newSound.pitch = s.pitch;

            newSound.loop = s.isLooping;

            if (newSound.playOnAwake)
            {
                newSound.Play();
            }

            s.audioSource = newSound;
        }


    }


    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);

        if (s==null)
        {
            Debug.LogError("Sound " + name + " not found!");
            return;
        }

        s.audioSource.Play();
    }

    public AudioSource GetSource(string name)
    {
        Sound s = Array.Find(sounds, s => s.name == name);

        return s.audioSource;
    }

    public void FadeIn(string name, float duration)
    {
        Sound s = Array.Find(sounds, s => s.name == name);

        if (s == null)
        {
            Debug.LogError("Sound " + name + " not found!");
            return;
        }

        if (s.audioSource == null)
        {
            Debug.LogError("AudioSource for sound " + name + " is not assigned!");
            return;
        }

        StartCoroutine(FadeInCoroutine(s.audioSource, s.globalVol, duration));
    }

    public void FadeOut(string name, float duration)
    {
        Sound s = Array.Find(sounds, s => s.name == name);

        if (s == null)
        {
            Debug.LogError("Sound " + name + " not found!");
            return;
        }

        if (s.audioSource == null)
        {
            Debug.LogError("AudioSource for sound " + name + " is not assigned!");
            return;
        }

        StartCoroutine(FadeOutCoroutine(s.audioSource, s.globalVol, duration));
    }

    private IEnumerator FadeInCoroutine(AudioSource audioSource, float global, float duration)
    {
        float startVolume = 0f;
        audioSource.volume = startVolume;
        audioSource.Play();

        while (audioSource.volume < global)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = global;
    }

    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float global, float duration)
    {
        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
