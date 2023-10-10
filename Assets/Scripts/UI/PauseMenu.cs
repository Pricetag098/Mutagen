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
            Time.timeScale = 1;
            PlayerSettingsHandler.instance.SaveGame();
        }

        else
        {
            Time.timeScale = 0;
            PlayerSettingsHandler.instance.LoadGame();
        }

    }
}
