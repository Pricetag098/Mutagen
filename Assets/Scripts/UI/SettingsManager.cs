using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static MapManager;

public class SettingsManager : MonoBehaviour
{
    static SettingsManager instance;

    public SettingsData settingsData = null;
    public AudioMixerGroup vfx,music,ambient;
    

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

        //vfx.audioMixer.se
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

public enum VolumeType
{
    SFX,
    music,
    ambient
}

public class SettingsData
{
    public float SFXVolume;
    public float musicVolume;
    public float ambientVolume;
    public float maxVolume;
    bool isFullScreen;
    int qualityIndex;

    public void SetupSettings(SettingsMenu menu)
    {
        menu.SetAllVolume(SFXVolume, musicVolume, ambientVolume);
        menu.SetFullScreen(isFullScreen);
        menu.SetQuality(qualityIndex);
    }

    public void SaveSettings(SettingsMenu menu)
    {
        SFXVolume = menu.SFXVolume;
        musicVolume = menu.musicVolume;
        ambientVolume = menu.ambientVolume;
        maxVolume = menu.maxVolume;
        isFullScreen = menu.isFullscreen;
        qualityIndex = menu.qualityIndex;
    }
}
