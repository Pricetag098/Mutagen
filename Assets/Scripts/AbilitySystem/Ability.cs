using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Empty")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    protected AbilityCaster caster;

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
