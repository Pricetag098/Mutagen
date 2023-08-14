using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityPickupInteractable : Interactable
{
    public Ability ability;
	[SerializeField] GameObject hoverText;
	[SerializeField] Image icon;
	
	public override void OnHover()
	{
		hoverText.SetActive(true);
	}
	public override void ExitHover()
	{
		if(hoverText != null)
		hoverText.SetActive(false);
	}
	public override void Interact(Interactor interactor)
	{
		interactor.GetComponent<PlayerAbilityCaster>().EquipAbility(ability);
		interactor.hasTarget = false;
		Destroy(transform.parent.gameObject);
	}
	private void Awake()
	{
		icon.sprite = ability.icon;
	}
}
