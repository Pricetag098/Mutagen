using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSliderFix : MonoBehaviour
{
    public VolumeSlider volume;
    public Slider slider;

    private void Start()
    {
        volume = GetComponent<VolumeSlider>();
        slider = GetComponent<Slider>();
    }

    public void SetSlider(float val)
    {
        Debug.Log("SETTING");
        volume.SetFloat(val);
    }

}
