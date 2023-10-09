using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SettingsData
{
    public float SFXVolume;
    public float musicVolume;
    public float ambientVolume;
    public float maxVolume;
    public bool isFullScreen;
    public int qualityIndex;

    //public void SetupSettings(SettingsMenu menu)
    //{
    //    menu.SetAllVolume(SFXVolume, musicVolume, ambientVolume);
    //    menu.SetFullScreen(isFullScreen);
    //    menu.SetQuality(qualityIndex);
    //}

    //public void SaveSettings(SettingsMenu menu)
    //{
    //    SFXVolume = menu.SFXVolume;
    //    musicVolume = menu.musicVolume;
    //    ambientVolume = menu.ambientVolume;
    //    maxVolume = menu.maxVolume; //remove
    //    isFullScreen = menu.isFullscreen;
    //    qualityIndex = menu.qualityIndex;
    //}

    //public void GetObjectData(SerializationInfo info, StreamingContext context)
    //{
    //    throw new NotImplementedException();
    //}
}
