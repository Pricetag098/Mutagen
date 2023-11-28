using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    //refs
    Renderer[] renderers;
    Health health;

    //stats
    float onHitFlashTime = 0.4f;
    bool hitEffect;
    float hitFlashTimer;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        health = GetComponentInParent<Health>();
        health.OnHit += OnHit;
    }

    private void FixedUpdate()
    {

        //on hit effect for player feedback
        if (!hitEffect)
            return;

        if (Time.time - hitFlashTimer > onHitFlashTime)
        {
            for(int i = 0; i < renderers.Length; i++)
                renderers[i].material.SetFloat("_RimLight", 0);
            hitEffect = false;
        }
    }

    void OnHit(DamageData data)
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.SetFloat("_RimLight", 1);
            renderers[i].material.SetColor("_RimLightColor", new Color(1, 0, 0, 5));
        }


        hitEffect = true;
        hitFlashTimer = Time.time;

    }



}
