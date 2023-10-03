using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public SettingsMenu menu;


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menu.gameObject.SetActive(!menu.transform.gameObject.active);

            if (!menu.gameObject.active)
            {
                Debug.Log("Close");
                MapManager.SaveSettings(menu);
                Time.timeScale = 1;
            }
            else
            {
                Debug.Log("Open");
                MapManager.SetSettings(menu);
                Time.timeScale = 0;
            }
        }
    }
}
