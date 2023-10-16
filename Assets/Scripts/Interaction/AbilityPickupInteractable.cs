using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityPickupInteractable : Interactable
{
    public Ability ability;
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
	public override void Interact(Interactor interactor)
	{
		Debug.Log("Interact");
		PlayerAbilityCaster player = interactor.GetComponent<PlayerAbilityCaster>();
		if (player.abilitySelector.open)
			return;
		player.EquipAbility(ability);
		interactor.hasTarget = false;
		Destroy(agent.transform.parent.gameObject);
	}
	private void Awake()
	{
        //icon.sprite = ability.icon;
	}

	private void Start()
	{
        //temp Jason Code
        ability = agent.manager.dropPool[0];
    }
}
