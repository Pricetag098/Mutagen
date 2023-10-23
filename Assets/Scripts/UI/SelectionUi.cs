using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

public class SelectionUi : MonoBehaviour
{

    SelectionButton[] buttons;
    Sequence openSequence;

    Sequence closeSequence;
    public float openTime = 0.1f;
    public float buttonEntryTime = .1f;
    public float buttonEntranceDelay = .05f;
    //public float buttonShowDelay = 1;
    public Vector3 buttonOffset;
    public Volume ppVolume;
    private void Start()
    {
        buttons = GetComponentsInChildren<SelectionButton>();

        Close();
		DOTween.Kill(this, true);
	}


    public void OpenWith(Ability[] abilities)
    {

    }


    [ContextMenu("open")]
    public void Open()
    {
        DOTween.Kill(this, true);
        openSequence = DOTween.Sequence(this);
		for (int i = 0; i < buttons.Length; i++)
		{
			openSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition + buttonOffset, 0));
		}
		openSequence.Append(transform.DOScaleX(1, openTime));
        openSequence.Join(DOTween.To(() => ppVolume.weight, x => ppVolume.weight = x, 1, openTime));
		
		//openSequence.AppendInterval(openTime);
		for (int i = 0; i < buttons.Length; i++)
        {
			
			openSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition, buttonEntryTime)).SetEase(Ease.InSine);
            openSequence.AppendInterval(buttonEntranceDelay);
        }
        
    }
    [ContextMenu("close")]
    public void Close()
    {
        DOTween.Kill(this,true);
        closeSequence = DOTween.Sequence(this);


        for (int i = buttons.Length-1; i >= 0; i--)
        {

            closeSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition + buttonOffset, buttonEntryTime)).SetEase(Ease.InSine);

            //closeSequence.AppendInterval(buttonEntranceDelay);

        }
        //closeSequence.AppendInterval(buttonEntryTime);

        closeSequence.Append(transform.DOScaleX(0, openTime));
        closeSequence.Join(DOTween.To(() => ppVolume.weight, x => ppVolume.weight = x, 0, openTime));


    }


   
    void Update()
    {
        
    }
}
