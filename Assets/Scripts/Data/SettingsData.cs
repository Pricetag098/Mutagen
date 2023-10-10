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
}
