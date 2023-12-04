using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Header("Refs")]
    public VolumeSlider[] sliders;
    public QualityDropDown dropDown;
    public FullScreenBool fullScreen;

    
    public GameObject firstThing;
	void Start()
    {
        
        //DontDestroyOnLoad(gameObject);
        
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
	private void OnEnable()
	{
	    

	}
	private void OnDisable()
	{
		PlayerAim.UseMouse = PlayerAim.UseMouse;
	}
	public void onClose()
    {
        Debug.Log("Save");
        PlayerSettingsHandler.instance.SaveGame();
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
        SetAllVolume(1);
        SetQuality(2);
        SetFullScreen(true);
    }

    public void onQuit()
    {
        Application.Quit();
    }
	private void Update()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

    
}
