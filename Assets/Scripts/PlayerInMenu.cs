using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInMenu : MonoBehaviour
{

    public Rigidbody player;
    public float animationTime;
    // Start is called before the first frame update
    void Awake()
    {
        DisablePlayer();
    }

    void DisablePlayer()
    {
        player.isKinematic = true;
        player.GetComponent<PlayerAim>().enabled = false;
        player.GetComponentInChildren<Animator>().SetTrigger("Sit");
    }

    void EnablePLayer()
    {
        
        player.GetComponentInChildren<Animator>().SetTrigger("Stand");
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(animationTime);
        sequence.AppendCallback(() =>
        {
            player.isKinematic = false;
            player.GetComponent<PlayerAim>().enabled = true;
        });
    }

}
