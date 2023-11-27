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
	public SelectionUi abilitySelector;
	[HideInInspector]public AbilityCaster caster;
	PlayerAim aim;
	PlayerMovement movement;
    [SerializeField] Optional<AbilityDisplay> display;

	private void Awake()
	{
		caster = GetComponent<AbilityCaster>();
	}
	private void Start()
	{
		aim = GetComponent<PlayerAim>();
		movement = GetComponent<PlayerMovement>();
		
		if (display.Enabled)
			display.Value.UpdateUI(caster.abilities);
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
        //Check if none of the abilitys are casting
        if (CanCast(-1))
        {
            caster.UpdateDirection(movement.movementDir);
        }

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
	

	bool CanCast(int index)
	{
		bool cast = true;
		for(int i = 0; i < caster.abilities.Length;i++)
		{
			if (i == index)
				continue;
			if (caster.abilities[i].castState == Ability.CastState.casting)
				cast = false;
		}
		return cast;
	}
	
	bool TryCast(InputAction action, int index)
	{
		
		
		switch (caster.abilities[index].castType)
		{
			case Ability.CastTypes.passive:
				movement.timeSinceLastInteruption = 0;
				
				caster.CastAbility(index, CreateCastData());
				
				break;
			case Ability.CastTypes.hold:
				if (action.IsPressed())
				{
                    movement.timeSinceLastInteruption = 0;
					if (CanCast(index))
					{
                        caster.CastAbility(index, CreateCastData());
						return true;
                    }
						
                }
					
				break;
			case Ability.CastTypes.press:
				if (action.WasPressedThisFrame())
				{
                    movement.timeSinceLastInteruption = 0;
					if (CanCast(index))
					{
						caster.CastAbility(index, CreateCastData());
						return true;
					}
						
                }	
				break;
			case Ability.CastTypes.disabled:
				break;

				
		}
        return false;

    }

	Ability.CastData CreateCastData()
	{
		Ability.CastData data = new Ability.CastData();
		data.origin = movement.orientation.position;
		data.aimDirection = aim.aimDir;
		data.moveDirection = movement.body.transform.forward;
		data.effectOrigin = castOrigin;
		return data;
	}

	bool TryEquipAbility(Ability ability)
	{
		for(int i = 0; i < caster.abilities.Length; i++)
		{
			if(caster.abilities[i].abilityName == caster.baseAbility.abilityName)
			{
				if (ability.slotMask.HasFlag((Ability.SlotMask)Mathf.Pow(2,i)))
				{
					Debug.Log("Player");
					SetAbility(ability,i);
					
					return true;
				}
				
			}
		}
		return false;
	}

	//public void EquipAbility(Ability ability)
	//{
	//	if (!TryEquipAbility(ability))
	//	{
	//		abilitySelector.Open(ability);
	//	}
		
	//}
	public void SetAbility(Ability ability, int index)
	{
		caster.SetAbility(ability, index);
		if (display.Enabled)
			display.Value.UpdateUI(caster.abilities);
	}

	public void SetAllAbilities(Ability[] abilities)
	{
		caster.SetAllAbilities(abilities);
		if (display.Enabled)
			display.Value.UpdateUI(caster.abilities);
	}
}


