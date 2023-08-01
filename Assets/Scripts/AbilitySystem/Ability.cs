using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Empty")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    protected AbilityCaster caster;
    
    [System.Flags]
	public enum SlotMask 
    { 
        basic = 1,
        range = 2,
        dash = 4,
        ability2 = 8,
        ability3 = 16
    }

    public SlotMask slotMask;
	public enum CastTypes 
    {
        press,
        hold,
        passive,
        disabled
    }
    public CastTypes castType;
	public void Equip(AbilityCaster abilityCaster)
    {
        caster = abilityCaster;
        OnEquip();
    }

    protected virtual void OnEquip()
    {

    }

    public void UnEquip()
    {
        
        OnUnEquip();
    }

    protected virtual void OnUnEquip()
    {

    }


    public virtual void Tick()
    {
        
    }

    public virtual void FixedTick()
    {

    }

    public virtual void Cast(CastData data)
    {

    
    }

    public virtual float GetCoolDownPercent()
	{
        return 1;
	}


    public virtual void OnDrawGizmos()
	{

	}



    public struct CastData 
    {
        public Transform effectOrigin;
        public Vector3 origin;
        public Vector3 aimDirection;
        public Vector3 moveDirection;

    }

}
