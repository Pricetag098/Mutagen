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

    protected Vector3 GetAssistedAimDir(Vector3 aimDir,LayerMask targetLayer,float maxRange, float minDirectionValue  = -1,float directionWeight = 1,float distanceWeight = 1, float priorityWeight =1,float assistWeight = 1)
	{
        if (assistWeight == 0)
            return aimDir;
        
        Collider[] colliders = Physics.OverlapSphere(caster.transform.position, maxRange, targetLayer);

        Vector3 bestTarget = aimDir;
        float bestVal = float.NegativeInfinity;
        for(int i = 0; i < colliders.Length; i++)
		{
            AimAssistTarget target;
            if(colliders[i].TryGetComponent(out target))
			{
                Vector3 casterToTarget = target.transform.position - caster.transform.position;
                casterToTarget.y = 0;
                Vector3 casterToTargetNorm = casterToTarget.normalized;
                float directionValue = Vector3.Dot(aimDir, casterToTargetNorm) * directionWeight * target.size;
                if (directionValue < minDirectionValue)
                    continue;
                float val = directionValue + (1 / casterToTarget.magnitude) * distanceWeight + target.priority;
                if(val > bestVal)
				{
                    bestVal = val;
                    bestTarget = casterToTargetNorm;
				}

            }
		}
        return Vector3.Lerp(aimDir,bestTarget,assistWeight).normalized;
	}

}
