using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    AbilityCaster caster;

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

    public struct CastData 
    {
        public Transform effectOrigin;
        public Vector3 origin;
        public Vector3 direction;
    }

}
