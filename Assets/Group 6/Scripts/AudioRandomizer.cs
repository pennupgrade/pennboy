using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    public AudioSource audioSource1;  // First audio source
    public AudioSource audioSource2;  // Second audio source

    void Start()
    {
        PlayRandomAudio();
    }

    // Method to randomly choose and play one of the audio sources
    void PlayRandomAudio()
    {
        int randomChoice = UnityEngine.Random.Range(0, 2);  // Randomly select 0 or 1
        Debug.Log(randomChoice);

        if (randomChoice == 0)
        {
            audioSource1.Play();
        }
        else
        {
            audioSource2.Play();
        }
    }
}
