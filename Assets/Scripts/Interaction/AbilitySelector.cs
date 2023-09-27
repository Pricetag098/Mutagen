using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AbilitySelector : MonoBehaviour
{
    Ability heldAbility;
    Button[] buttons;
    [SerializeField] PlayerAbilityCaster abilityCaster;
    [SerializeField] Image heldIcon;
    public bool open;
    CanvasGroup canvasGroup;
    [SerializeField] float fadeTime;
    [SerializeField] Transform buttonParent;
    [SerializeField]TextMeshProUGUI title,description;
    // Start is called before the first frame update
    void Awake()
    {
        buttons = buttonParent.GetComponentsInChildren<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Cancel()
    {
        if(heldAbility.pickupPrefab.Enabled)
        {
            GameObject g = Instantiate(heldAbility.pickupPrefab.Value);
            g.transform.position = abilityCaster.transform.position;
        }
        Close();
    }

    void Close()
    {
		Time.timeScale = 1;
		open = false;
		transform.DOScale(Vector3.zero, fadeTime);
		canvasGroup.DOFade(0, fadeTime);
		canvasGroup.interactable = false;
	}

    public void Select(int index)
	{
        abilityCaster.SetAbility(heldAbility,index);
        //gameObject.SetActive(false);
        Close();
	}

    public void Open(Ability ability)
	{
        //Time.timeScale = 0;
        //gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, fadeTime);
		canvasGroup.DOFade(1, fadeTime);
		canvasGroup.interactable = true;
		heldAbility = ability;
        for(int i = 0; i < buttons.Length; i++)
		{
            Button button = buttons[i];
            button.interactable = ability.slotMask.HasFlag((Ability.SlotMask)Mathf.Pow(2, i));
            button.image.sprite = abilityCaster.caster.abilities[i].icon;
        }
        heldIcon.sprite = ability.icon;
        title.text = ability.abilityName;
        description.text = ability.abilityDescription;
        open = true;
        
	}
	private void OnDestroy()
	{
        DOTween.KillAll();
	}
}
