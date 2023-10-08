using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputActionProperty pauseAction;
    public SettingsMenu menu;


    private void Start()
    {
        pauseAction.action.performed += Pause;
    }

    private void Update()
    {

    }

    void Pause(InputAction.CallbackContext context)
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
