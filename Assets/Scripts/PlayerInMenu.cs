using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInMenu : MonoBehaviour
{

    public Rigidbody player;
    public CanvasGroup canvas,playerUi;
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
    }

    public void EnablePLayer()
    {
        
        player.GetComponentInChildren<Animator>().SetTrigger("Stand");
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvas.DOFade(0,fadeTime));
        sequence.Join(playerUi.DOFade(1, fadeTime));
        sequence.AppendInterval(animationTime);
        sequence.AppendCallback(() =>
        {
            player.isKinematic = false;
            player.GetComponent<PlayerAim>().enabled = true;
            player.GetComponent<PlayerAbilityCaster>().enabled = true;
        });
    }

}
