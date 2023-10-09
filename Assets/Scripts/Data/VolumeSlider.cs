using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSlider : MonoBehaviour,IDataPersistance<SettingsData>
{
    public AudioMixer audioMixer;
    public string path;
    float volume;
    public void SetFloat(float f)
    {
        volume = Mathf.Log10(f) * 20;
        audioMixer.SetFloat(path,volume);
    }
    void IDataPersistance<SettingsData>.SaveData(ref SettingsData data)
    {
        data.ambientVolume = volume;
    }

    void IDataPersistance<SettingsData>.LoadData(SettingsData data)
    {
        volume = data.ambientVolume;
        SetFloat(volume);
    }

}
