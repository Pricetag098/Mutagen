using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectionUi : MonoBehaviour
{
    [SerializeField]SelectionButton[] buttons;
    Sequence openSequence;
    public bool open;
    Sequence closeSequence;
    public float openTime = 0.1f;
    public float buttonEntryTime = .1f;
    public float buttonEntranceDelay = .05f;
    CanvasGroup group;
    PlayerAbilityCaster playerAbilityCaster;
    //public float buttonShowDelay = 1;
    public Vector3 buttonOffset;
    public Volume ppVolume;
    [SerializeField]Ability[] abilityOptions;
    private void Awake()
    {
        playerAbilityCaster = FindObjectOfType<PlayerAbilityCaster>();
        buttons = GetComponentsInChildren<SelectionButton>();
        group = GetComponent<CanvasGroup>();

        DOTween.defaultTimeScaleIndependent = true;
	}
	private void Start()
	{
		DOTween.defaultTimeScaleIndependent = true;
		Close();
		DOTween.Kill(this, true);
	}

	public void OpenWith(Ability[] abilities)
    {
        for(int i = 0; i < buttons.Length || i < abilities.Length; i++)
        {
            if((int)abilities[i].slotMask < playerAbilityCaster.caster.abilities.Length)
            {
                buttons[i].SetValues(abilities[i],
                playerAbilityCaster.caster.abilities[(int)abilities[i].slotMask]);
            }
            else
            {
                Debug.Log(abilities[i]);
            }
            
        }
        abilityOptions = abilities;
        Open();
    }

    public void Select(int index)
    {
        if (!open)
            return;
        playerAbilityCaster.SetAbility(abilityOptions[index],(int)abilityOptions[index].slotMask);
        //gameObject.SetActive(false);
        Close();
    }
    [ContextMenu("open")]
    public void Open()
    {
        if (open)
            return;

        open = true;
        DOTween.Kill(this, true);
        openSequence = DOTween.Sequence(this);
        
		for (int i = 0; i < 3; i++)
		{
			openSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition + buttonOffset, 0));
		}
		openSequence.Append(transform.DOScaleX(1, openTime));
        openSequence.Join(DOTween.To(() => ppVolume.weight, x => ppVolume.weight = x, 0.35f, openTime));
        openSequence.Join(group.DOFade(1, openTime));
        //openSequence.Join(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, openTime));

        //openSequence.AppendInterval(openTime);
        for (int i = 0; i < buttons.Length; i++)
        {
			
			openSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition, buttonEntryTime)).SetEase(Ease.InSine);
            
            openSequence.AppendInterval(buttonEntranceDelay);
        }

        //openSequence.AsyncWaitForCompletion(() => { Time.timeScale = 0; });
        //openSequence.AppendCallback(() => { Time.timeScale = 0; });
    }
    [ContextMenu("close")]
    public void Close()
    {
        open = false;
        DOTween.Kill(this,true);
        closeSequence = DOTween.Sequence(this);

        for (int i = 2; i >= 0; i--)
        {

            closeSequence.Append(buttons[i].GetComponent<RectTransform>().DOAnchorPos(buttons[i].startPosition + buttonOffset, buttonEntryTime)).SetEase(Ease.InSine);

            //closeSequence.AppendInterval(buttonEntranceDelay);

        }
        //closeSequence.AppendInterval(buttonEntryTime);

        closeSequence.Append(transform.DOScaleX(0, openTime));
        closeSequence.Join(group.DOFade(0, 1));
        closeSequence.Join(DOTween.To(() => ppVolume.weight, x => ppVolume.weight = x, 0, openTime));
        
        //closeSequence.Join(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, openTime));

    }


   
    void Update()
    {
        
    }
}
