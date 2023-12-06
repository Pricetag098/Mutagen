using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInMenu : MonoBehaviour
{

    public Rigidbody player;
    public CanvasGroup canvas,playerUi;
    public GameObject[] partsToDisable;
    public float fadeTime;
    public float animationTime;
    // Start is called before the first frame update
    void Start()
    {
        DisablePlayer();
        
    }

    void DisablePlayer()
    {
        player.isKinematic = true;
        player.GetComponent<PlayerAim>().enabled = false;
        player.GetComponent<PlayerAbilityCaster>().enabled = false;
        player.GetComponentInChildren<Animator>().SetTrigger("Sit");
        //playerUi.gameObject.SetActive(false);
        foreach(GameObject disable in partsToDisable)
        disable.SetActive(false);
    }

    public void EnablePLayer()
    {
        //playerUi.gameObject.SetActive(true);
        foreach (GameObject disable in partsToDisable)
            disable.SetActive(true);
        player.GetComponentInChildren<Animator>().SetTrigger("Stand");
        canvas.interactable = false;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvas.DOFade(0,fadeTime));
        sequence.Join(playerUi.DOFade(1, fadeTime));
        sequence.AppendInterval(animationTime);
        sequence.AppendCallback(() =>
        {
            player.isKinematic = false;
            player.GetComponent<PlayerAim>().enabled = true;
            player.GetComponent<PlayerAbilityCaster>().enabled = true;
			PlayerAim.UseMouse = PlayerAim.UseMouse;
			enabled = false;
        });

    }
	
	private void Update()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

}
