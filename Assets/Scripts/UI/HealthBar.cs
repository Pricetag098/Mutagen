using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Health health;
    public Image hBar;
    public Image dBar;
    public TMP_Text text;

    private void Start()
    {
        health.OnHit += OnHit;

    }


    // Update is called once per frame
    void Update()
    {
        hBar.fillAmount = health.health/health.maxHealth;
        text.text = (int)health.health + " / " + health.maxHealth;
    }

    public void OnHit(DamageData damage)
    {
        DoTween();
    }


    //Async example
    public Image hitEffect;

    public async void DoTween()
    {
        await hitEffect.DOFade(1, 1).AsyncWaitForCompletion();
        await dBar.DOFillAmount(hBar.fillAmount, 3).AsyncWaitForCompletion();
        await hitEffect.DOFade(0, 1).AsyncWaitForCompletion();
    }
}
