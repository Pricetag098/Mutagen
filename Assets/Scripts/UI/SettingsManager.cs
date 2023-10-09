using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum VolumeType
{
    SFX,
    music,
    ambient
}

public class SettingsManager : MonoBehaviour
{
    static SettingsManager instance;

    public SettingsData settingsData = null;
    public AudioMixer sfx,music,ambient;
    
    private void Awake()
    {
        //if already instanced, destroy new gameObject
        if(instance != null)
        {
            Destroy(gameObject);
            
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            settingsData = null;
        }
    }

    public static void SetSettings(SettingsMenu menu)
    {
        if (instance.settingsData != null)
        {
            instance.settingsData.SetupSettings(menu);   
        }
    }

    public static float GetVolume(VolumeType type)
    {
        switch (type)
        {
            case VolumeType.SFX:
                return instance.settingsData.SFXVolume / instance.settingsData.maxVolume;
            case VolumeType.music:
                return instance.settingsData.musicVolume / instance.settingsData.maxVolume;
            case VolumeType.ambient:
                return instance.settingsData.ambientVolume / instance.settingsData.maxVolume;
            default:
                return instance.settingsData.SFXVolume / instance.settingsData.maxVolume;
        }
    }

    public static void SaveSettings(SettingsMenu menu)
    {
        if (instance.settingsData != null)
        {
            instance.settingsData.SaveSettings(menu);
        }
        else
        {
            instance.settingsData = new SettingsData();
            instance.settingsData.SaveSettings(menu);
        }
    }
}