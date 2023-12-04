using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelHover : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public float timeToTween;
    public float tweenScale;
    RingSpinner ringSpinner;
    public float speed = .1f;
    private void Awake()
    {
        ringSpinner = GetComponentInParent<RingSpinner>();
    }

    public void TweenScale(float variable)
    {
        transform.DOScale(variable, timeToTween);
    }

    public void TweenSelect()
    {
        //transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), timeToTween).OnComplete(() => { transform.DOScale(1, 0); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.To(() => ringSpinner.rotationSpeed, x => ringSpinner.rotationSpeed = x, 10, speed);
        transform.DOKill();
        TweenScale(tweenScale);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOKill();
        TweenSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.To(() => ringSpinner.rotationSpeed, x => ringSpinner.rotationSpeed = x, 10, speed);
        transform.DOKill();
        TweenScale(1);
    }
}


