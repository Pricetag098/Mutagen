using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SelectionButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    public float timeToTween;
    public float tweenScale;
    public Vector3 startPosition;
    Button button;
    public int index;
    [SerializeField]Image oldIcon, newIcon;
    [SerializeField]TextMeshProUGUI title, description;
    private void Awake()
    {
        button = GetComponent<Button>();
        startPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void SetValues(Ability newAbility,Ability oldAbiliy)
    {
        title.text = newAbility.abilityName + " > " + oldAbiliy.abilityName;
        description.text = newAbility.abilityDescription;
        oldIcon.sprite = oldAbiliy.icon;
        newIcon.sprite = newAbility.icon;
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
        GetComponentInParent<SelectionUi>().Select(index);
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
