using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputActionProperty pauseAction;
    public SettingsMenu menu;	

	private void Start()
    {
		PlayerSettingsHandler.instance.ReloadTargets();
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

	private void OnDestroy()
	{
		pauseAction.action.performed -= Pause;
	}

	void Pause(InputAction.CallbackContext context)
    {
        menu.gameObject.SetActive(!menu.gameObject.active);
        //close
        if (!menu.gameObject.active)
        {
            Time.timeScale = 1;
            PlayerSettingsHandler.instance.SaveGame();
        }
        //open
        else
        {
            Time.timeScale = 0;
            PlayerSettingsHandler.instance.LoadGame();
        }
    }
}
