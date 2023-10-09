using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour, IDataPersistance<SettingsData>
{
    public AudioMixerGroup audioMixer;
    public Slider slider;
    public string path;
    float volume;

    private void Start()
    {
        slider = GetComponent<Slider>();    
    }


    public void SetFloat(float f)
    {
        //volume = Mathf.Log10(f) * 20;
        volume = f;
        slider.value = volume;
        audioMixer.audioMixer.SetFloat(path,volume);
    }
    void IDataPersistance<SettingsData>.SaveData(ref SettingsData data)
    {
        data.SFXVolume = volume;
    }

    void IDataPersistance<SettingsData>.LoadData(SettingsData data)
    {
        volume = data.SFXVolume;
        SetFloat(volume);
    }

}
