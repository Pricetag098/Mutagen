using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAbilityCaster : MonoBehaviour
{
    [SerializeField] InputActionProperty abilityBasic;
    [SerializeField] InputActionProperty abilityDash;
    [SerializeField] InputActionProperty abilityAction2;
    [SerializeField] InputActionProperty abilityAction3;
    [SerializeField] InputActionProperty abilityAction4;
	public Transform castOrigin;
	public AbilitySelector abilitySelector;
	[HideInInspector]public AbilityCaster caster;
	PlayerAim aim;
	PlayerMovement movement;
    
	private void Start()
	{
		aim = GetComponent<PlayerAim>();
		movement = GetComponent<PlayerMovement>();
		caster = GetComponent<AbilityCaster>();
	}
	private void OnEnable()
	{
		abilityBasic.action.Enable();
		abilityDash.action.Enable();
		abilityAction2.action.Enable();
		abilityAction3.action.Enable();
		abilityAction4.action.Enable();
	}

	private void Update()
	{
		TryCast(abilityBasic.action, 0);
		TryCast(abilityDash.action, 2);
		TryCast(abilityAction2.action, 1);
		TryCast(abilityAction3.action, 3);
		TryCast(abilityAction4.action, 4);
	}
	private void OnDisable()
	{
		abilityBasic.action.Disable();
		abilityDash.action.Disable();
		abilityAction2.action.Disable();
		abilityAction3.action.Disable();
		abilityAction4.action.Disable();
	}
	

	void TryCast(InputAction action, int index)
	{
		
		
		switch (caster.abilities[index].castType)
		{
			case Ability.CastTypes.passive:
				caster.CastAbility(index, CreateCastData());
				break;
			case Ability.CastTypes.hold:
				if (action.IsPressed())
					caster.CastAbility(index, CreateCastData());
				break;
			case Ability.CastTypes.press:
				if (action.WasPressedThisFrame())
					caster.CastAbility(index, CreateCastData());
				break;
			case Ability.CastTypes.disabled:
				break;
		}

	}

	Ability.CastData CreateCastData()
	{
		Ability.CastData data = new Ability.CastData();
		data.origin = movement.orientation.position;
		data.aimDirection = aim.aimDir;
		data.moveDirection = movement.movementDir;
		data.effectOrigin = castOrigin;
		return data;
	}

	bool TryEquipAbility(Ability ability)
	{
		for(int i = 0; i < caster.abilities.Length; i++)
		{
			if(caster.abilities[i].GetType() == caster.baseAbility.GetType())
			{
				if (ability.slotMask.HasFlag((Ability.SlotMask)Mathf.Pow(2,i)))
				{
					caster.SetAbility(ability,i);
					return true;
				}
				
			}
		}
		return false;
	}

	public void EquipAbility(Ability ability)
	{
		if (!TryEquipAbility(ability))
		{
			abilitySelector.Open(ability);
		}
	}
	

}
