using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer aMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        SetGameVolume();
    }

    public void SetGameVolume()
    {
        float volume = volumeSlider.value;

        // Audio Mixe value changes Logarithmicly (0.01, 0.1, 1, 1.0, 1.00)
        // Slider value changes linearly (-2, -1, 0, 1, 2)
        aMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }
}
