using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None,
    Gravity,
    Time,
    Light
}

[CreateAssetMenu(menuName = "Abilities/Empty")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    protected AbilityCaster caster;
    public delegate void CastDelegate(CastData data);
    public Element element;
    public CastDelegate OnCast;
    public Optional<GameObject> pickupPrefab;

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

    public void UnEquip(Ability replacement)
    {
        replacement.OnCast += OnCast;
        OnUnEquip(replacement);
    }

    protected virtual void OnUnEquip(Ability replacement)
    {

    }


    public virtual void Tick()
    {
        
    }

    public virtual void FixedTick()
    {

    }

    public void Cast(CastData data)
    {
        DoCast(data);
    }
    protected virtual void DoCast(CastData data)
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

    protected virtual DamageData CreateDamageData(float damage)
    {
        DamageData data = new DamageData();
        data.damage = damage;
        data.type = element;
        return data;
    }
}
