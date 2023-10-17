using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SelectionButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public float timeToTween;
    public float tweenScale;
    public Vector3 startPosition;
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        startPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void TweenScale(float variable)
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
        TweenScale(tweenScale);
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
        TweenScale(1);
    }
}
