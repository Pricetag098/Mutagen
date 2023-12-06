using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public RectTransform rectTransform;
    public CanvasGroup group;
    public float scrollTime,fadeTime;
    public RectTransform EndPos;
    void Open()
    {
        Sequence s = DOTween.Sequence(this);
        s.Append(group.DOFade(1, fadeTime));
        s.Append(rectTransform.DOAnchorPos(EndPos.anchoredPosition, scrollTime));
        s.AppendCallback(() => MapManager.LoadNext());
    }
}
