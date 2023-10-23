	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityPickupInteractable : Interactable
{
    public Ability[] abilitys;
	[SerializeField] GameObject hoverText;
	[SerializeField] Image icon;

	//Jason Code
	public Enemy agent;
	
	public override void OnHover()
	{
		//hoverText.SetActive(true);
	}
	public override void ExitHover()
	{
		if(hoverText != null)
		hoverText.SetActive(false);
	}

	public void SetAbilities(Ability[] other)
	{
		for(int i = 0; i < other.Length; i++)
		{
			abilitys[i] = other[i];
		}
	}

	public override void Interact(Interactor interactor)
	{
		Debug.Log("Interact");
		PlayerAbilityCaster player = interactor.GetComponent<PlayerAbilityCaster>();
		if (player.abilitySelector.open)
			return;
		//player.EquipAbility(ability);
		interactor.hasTarget = false;
		//Destroy(agent.transform.parent.gameObject);
		enabled = false; 
	}
	private void Awake()
	{
        //icon.sprite = ability.icon;
	}

}
