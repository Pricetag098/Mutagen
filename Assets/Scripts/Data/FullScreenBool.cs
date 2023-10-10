using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenBool : MonoBehaviour, IDataPersistance<SettingsData>
{
    bool isFullscreen;
    public Toggle toggle;

    public void SetBool(bool set)
    {
        isFullscreen = set;
        Screen.fullScreen = isFullscreen;
    }

    void IDataPersistance<SettingsData>.SaveData(ref SettingsData data)
    {
        data.isFullScreen = isFullscreen;
    }

    void IDataPersistance<SettingsData>.LoadData(SettingsData data)
    {
        isFullscreen = data.isFullScreen;
        SetBool(isFullscreen);
        toggle.isOn = isFullscreen;
    }
}
