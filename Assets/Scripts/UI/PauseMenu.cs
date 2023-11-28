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
    public Volume PP;
	
	

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
            PlayerSettingsHandler.instance.SaveGame();
            PP.weight = 0;
            Time.timeScale = 1;


        }
        //open
        else
        {
            PlayerSettingsHandler.instance.LoadGame();
            PP.weight = 1;
            Time.timeScale = 0;
        }

    }


    
}
