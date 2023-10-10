using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualityDropDown : MonoBehaviour, IDataPersistance<SettingsData>
{
    int index;
    public TMP_Dropdown dropDown;

    public void SetIndex(int val)
    {
        index = val;
        QualitySettings.SetQualityLevel(index);
    }

    void IDataPersistance<SettingsData>.SaveData(ref SettingsData data)
    {
        data.qualityIndex = index;
    }

    void IDataPersistance<SettingsData>.LoadData(SettingsData data)
    {
        index = data.qualityIndex;
        SetIndex(index);
        dropDown.value = index;
    }
}
