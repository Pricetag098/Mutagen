using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None,
    Light,
    Gravity,
    Tech,
    Player
}

[CreateAssetMenu(menuName = "Abilities/Empty")]
public class Ability : ScriptableObject
{
    public string abilityName;
    [Multiline] public string abilityDescription;
    public Sprite icon;
    protected AbilityCaster caster;
    public delegate void CastDelegate(CastData data);
    public Element element;
    public CastDelegate OnCast;
    public Optional<GameObject> pickupPrefab;
    protected CastData lastCastData;
    //[System.Flags]
	public enum SlotMask 
    { 
        basic,
        range,
        dash,
        specialUtility,
        specialDamage
    }

    public SlotMask slotMask;
	public enum CastTypes 
    {
        press,
        hold,
        passive,
        disabled
    }

    public enum CastState 
    {
        none,
        windUp,
        casting,
        windDown
    }
    public CastState castState;
    public CastTypes castType;

    public float windUptime;
    public float windDownTime;
    Timer windUpTimer,windDownTimer;
    public float moveSpeedModifier;
	public void Equip(AbilityCaster abilityCaster)
    {
        caster = abilityCaster;
        windUpTimer = new Timer(windUptime, false);
        windDownTimer = new Timer(windDownTime, false);
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

    public void Tick()
    {
        
        switch (castState)
        {
            case CastState.none:
                break;
            case CastState.windUp:
				windUpTimer.Tick();
				if (windUpTimer.complete)
				{
					castState = CastState.casting;
					DoCast(lastCastData);
				}
				break;
            case CastState.casting: 
            break;
            case CastState.windDown: 
                windDownTimer.Tick();
                if(windDownTimer.complete)
                {
                    castState = CastState.none;
                    caster.ChangeSpeed(moveSpeedModifier);
                }
                break;
        }
		OnTick();

	}

    public virtual void OnTick()
    {
        
    }

    public virtual void FixedTick()
    {

    }

    public void Cast(CastData data)
    {
        switch (castState)
        {
            case CastState.none:
                castState = CastState.windUp;
                windUpTimer.Reset();
                lastCastData = data;
				caster.ChangeSpeed(-moveSpeedModifier);
				break;
            case CastState.casting:
				DoCast(data);
				break;
        }
        
    }

    protected void FinishCast()
    {
        windDownTimer.Reset();
        castState = CastState.windDown;
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
