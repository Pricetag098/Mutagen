using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSettings : MonoBehaviour, IDataPersistance<SettingsData>
{
	public Toggle t;
	
	public void LoadData(SettingsData data)
	{
		PlayerAim.UseMouse = data.useMouse;
		
		t.isOn = !data.useMouse;
	}

	public void SaveData(ref SettingsData data)
	{
		data.useMouse = PlayerAim.UseMouse;
	}


    public void Toggle(bool toggle)
	{
		PlayerAim.UseMouse = !toggle;
	}
}
