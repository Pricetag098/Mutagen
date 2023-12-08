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
    public float damageThreshhold = 300;
    public Optional<float> freezeframeTime;

    private void Start()
    {
        health.OnHit += OnHit;
        //update health on load
        hBar.fillAmount = health.health / health.maxHealth;
        text.text = (int)health.health + " / " + health.maxHealth;
        dBar.DOFillAmount(hBar.fillAmount, 3);
    }
    public void OnHit(DamageData damage)
    {
        if(health.health < damageThreshhold)
        {
        
            Sequence sequence = DOTween.Sequence();
            sequence.Append(hitEffect.DOFade(1, 1));
            sequence.Append(hitEffect.DOFade(0, 1));
            sequence.Append(dBar.DOFillAmount(hBar.fillAmount, 3));
        }
        if(freezeframeTime.Enabled)
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, freezeframeTime.Value).SetEase(Ease.InOutBounce).SetLoops(1,LoopType.Yoyo);

		hBar.fillAmount = health.health / health.maxHealth;
        text.text = (int)health.health + " / " + health.maxHealth;
        dBar.DOFillAmount(hBar.fillAmount, 3);

        if(damage.damage < 0 ){
            Sequence sequence = DOTween.Sequence();
            sequence.Append(healEffect.DOFade(1, 1));
            sequence.Append(healEffect.DOFade(0, 1));
            sequence.Append(dBar.DOFillAmount(hBar.fillAmount, 3));

        }

    }
    //Async example
    public Image hitEffect;
    public Image healEffect;
}
