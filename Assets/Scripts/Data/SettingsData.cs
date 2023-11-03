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
    public float maxVolume = -80;
    public bool isFullScreen;
    public int qualityIndex;
    public bool useMouse = true;
}
