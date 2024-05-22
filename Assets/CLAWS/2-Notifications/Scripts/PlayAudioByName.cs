using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayAudioByName : MonoBehaviour
{
    // Reference to the AudioSource component
    private static AudioSource audioSource;

    // Initialize the AudioSource component
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        EventBus.Subscribe<PlayAudio>(onAudioPlayed);
    }



    public void onAudioPlayed(PlayAudio e)
    {
        PlayAudio(e.audioName);
    }

    // Method to play an audio file by name
    public static void PlayAudio(string audioFileName)
    {
        // Load the AudioClip from the Resources folder
        string path = "Notification Audio Files/" + audioFileName;
        AudioClip clip = Resources.Load<AudioClip>(path);

        // Check if the AudioClip exists
        if (clip != null)
        {
            // Assign the AudioClip to the AudioSource and play it
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            // Log a warning if the AudioClip was not found
            Debug.LogWarning("Audio file not found: " + path);
        }
    }
}

