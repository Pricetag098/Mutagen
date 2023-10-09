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
        //menu = GetComponentInChildren<SettingsMenu>();
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

    void Pause(InputAction.CallbackContext context)
    {
        menu.gameObject.SetActive(!menu.transform.gameObject.active);

        if (!menu.gameObject.active)
        {
            SettingsManager.SaveSettings(menu);
            Time.timeScale = 1;
        }
        else
        {
            SettingsManager.SetSettings(menu);
            Time.timeScale = 0;
        }
    }
}
