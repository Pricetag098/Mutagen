using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenBool : MonoBehaviour, IDataPersistance<SettingsData>
{
    bool isFullscreen;

    public void SetBool(bool set)
    {
        isFullscreen = set;
    }

    public void LoadData(SettingsData data)
    {
        //data.i
    }

    public void SaveData(ref SettingsData data)
    {
        throw new System.NotImplementedException();
    }
}
