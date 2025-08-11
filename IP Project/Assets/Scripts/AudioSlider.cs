using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer mixer; //Assign MainAudioMixer
    public string volumeParameter;
    public Slider slider;
    public float multiplier = 30f; // Adjusts volume
    public bool saveSetting = true; // Save Player prefence
    string prefKey;
    void Awake()
    {
        prefKey = volumeParameter + "_Volume";
        if (slider == null) slider = GetComponent<Slider>();

        // Load saved volume
        if (PlayerPrefs.HasKey(prefKey))
        {
            slider.value = PlayerPrefs.GetFloat(prefKey);
        }
    }

    void OnEnable()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
        HandleSliderValueChanged(slider.value); // Apply immediately
    }

    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(HandleSliderValueChanged);
    }

    void HandleSliderValueChanged(float value)
    {
        // Convert linear slider [0,1] to logarithmic dB scale
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * multiplier;
        mixer.SetFloat(volumeParameter, dB);

        if (saveSetting)
            PlayerPrefs.SetFloat(prefKey, value);
    }

}
