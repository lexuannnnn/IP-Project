using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    AudioSource musicSource;
    
    [SerializeField]
    AudioSource brokenLightSource;

    [SerializeField]
    AudioSource fountainSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip brokenLight;
    public AudioClip fountain;


    /// <summary>
    /// Play background music on awake
    /// </summary>
    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayBrokenLightSFX(AudioClip clip)
    {
        brokenLightSource.PlayOneShot(clip);
    }

    public void PlayFountainSFX(AudioClip clip)
    {
        fountainSource.PlayOneShot(clip);
    }
}