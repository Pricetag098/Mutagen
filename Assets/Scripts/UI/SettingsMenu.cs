using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Refs")]
    public VolumeSlider[] sliders;
    public TMP_Dropdown dropdown;

     public float SFXVolume;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float ambientVolume;
    public float maxVolume = 0;
    [HideInInspector] public int qualityIndex;
    [HideInInspector] public bool isFullscreen;

    void Start()
    {
        //SetAllVolume(maxVolume);

        SettingsManager.SetSettings(this);

        //testing
        if (gameObject.active)
            gameObject.SetActive(false);
    }

    #region setSliders
    public void SetSFXVolume(float val) 
    {
        SFXVolume = val;
        sliders[0].SetFloat(SFXVolume);
    }
    public void SetMusicVolume(float val)
    {
        musicVolume = val;
        sliders[1].SetFloat(musicVolume);
    }
    public void SetAmbientVolume(float val)
    {
        ambientVolume = val;
        sliders[2].SetFloat(ambientVolume);
    }

    public void SetAllVolume(float val1, float val2, float val3)
    {
        SFXVolume = val1;
        sliders[0].SetFloat(SFXVolume);
        musicVolume = val2;
        sliders[1].SetFloat(musicVolume);
        ambientVolume = val3;
        sliders[2].SetFloat(ambientVolume);
    }

    public void SetAllVolume(float val)
    {
        SFXVolume = val;
        sliders[0].SetFloat(SFXVolume);
        musicVolume = val;
        sliders[1].SetFloat(musicVolume);
        ambientVolume = val;
        sliders[2].SetFloat(ambientVolume);
    }
    #endregion

    public void onClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetQuality(int index)
    {
        qualityIndex = index;
        dropdown.value = qualityIndex;
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullScreen(bool fullScreen)
    {
        isFullscreen = fullScreen;
        Screen.fullScreen = fullScreen;
    }

    public void SetDefault()
    {
        SetAllVolume(maxVolume);
        SetQuality(2);
        SetFullScreen(true);

        SettingsManager.SaveSettings(this);
    }

    public void onQuit()
    {
        Application.Quit();
    }

    public void SaveSettings()
    {
        SettingsManager.SaveSettings(this);
    }
}
