using UnityEngine;

public class FountainBehaviour : MonoBehaviour
{
    /// <summary>
    /// Access AudioManager instance
    /// </summary>
    AudioManager audioManager;

    void Start()
    {
        // Find the AudioManager in the scene by its tag
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    /// <summary>
    /// Plays sound effect for the fountain.
    /// </summary>
    public void PlayFountainSound()
    {
        audioManager.PlayFountainSFX(audioManager.fountain);
    }
}
