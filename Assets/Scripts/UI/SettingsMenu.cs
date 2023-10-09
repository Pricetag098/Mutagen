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
    public QualityDropDown dropDown;
    public FullScreenBool fullScreen;

    void Start()
    {

    }

    public void SetAllVolume(float val1, float val2, float val3)
    {
        sliders[0].SetFloat(val1);
        sliders[1].SetFloat(val2);
        sliders[2].SetFloat(val3);
    }

    public void SetAllVolume(float val)
    {
        sliders[0].SetFloat(val);
        sliders[1].SetFloat(val);
        sliders[2].SetFloat(val);
    }

    public void onClose()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetQuality(int index)
    {
        dropDown.SetIndex(index);
    }

    public void SetFullScreen(bool val)
    {
        fullScreen.SetBool(val);
    }

    public void SetDefault()
    {
        SetAllVolume(sliders[0].slider.maxValue);
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
