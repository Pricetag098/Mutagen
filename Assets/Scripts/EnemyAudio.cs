using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] Health health;


    private void Start()
    {
        health.OnHit += PlaySound;
    }

    void PlaySound(DamageData data)
    {
        SoundPlayer sound = GetComponent<SoundPlayer>();
        sound.Play();
    }

}
