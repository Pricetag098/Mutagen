
    using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class StartButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public float timeToTween;
    public float tweenScale;
    public float moveTime;
    public Ease ease;
    
    Button button;
    Vector3 initalScale;
    public Transform outline;
    private void Awake()
    {
        button = GetComponent<Button>();
        initalScale = transform.localScale;
    }

    

    public void TweenScale(Vector3 variable)
    {
        transform.DOScale(variable, timeToTween);
    }

    public void TweenSelect()
    {
        transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), timeToTween).OnComplete(() => { transform.DOScale(1, 0); });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        transform.DOKill();
        TweenScale(Vector3.one* tweenScale);
        outline.DOMove(transform.position,moveTime).SetEase(ease);
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.DOKill();
        
        TweenSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
            return;
        transform.DOKill();
        TweenScale(initalScale);
    }

    

    public void Quit()
    {
        Application.Quit();
    }
}
