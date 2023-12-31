using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeSlider : MonoBehaviour, IDataPersistance<SettingsData>
{
    public VolumeType volumeType;
    public AudioMixerGroup audioMixer;
    public Slider slider;
    public string path;
    public float volume;

    private void Start()
    {
        slider = GetComponent<Slider>();

        TestSliderFix[] slidFix = FindObjectsOfType<TestSliderFix>();
        for (int i = 0; i < slidFix.Length; i++)
            slidFix[i].SetSlider(volume);
    }

    public void SetFloat(float f)
    {
        
        volume = f;
        slider.value = volume;
        audioMixer.audioMixer.SetFloat(path, Mathf.Log10(f) * 20);
    }

    void IDataPersistance<SettingsData>.SaveData(ref SettingsData data)
    {
        switch (volumeType)
        {
            case VolumeType.SFX:
                data.SFXVolume = volume;
                break;
            case VolumeType.music:
                data.musicVolume = volume;
                break;
            case VolumeType.ambient:
                data.ambientVolume = volume;
                break;
        }
    }

    void IDataPersistance<SettingsData>.LoadData(SettingsData data)
    {
        switch (volumeType)
        {
            case VolumeType.SFX:
                volume = data.SFXVolume;
                break;
            case VolumeType.music:
                volume = data.musicVolume;
                break;
            case VolumeType.ambient:
                volume = data.ambientVolume;
                break;
        }
        SetFloat(volume);


    }
}
