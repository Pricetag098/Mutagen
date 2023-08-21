using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ranged")]
public class RangedAbility : Ability
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] bool releaseOnFullCharge;
    protected ObjectPooler projectileSpawner;

    protected float chargeTime;
    [SerializeField]protected float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;

    [SerializeField] AnimationCurve chargeDamageCurve = AnimationCurve.Linear(0,0,1,100);
    [SerializeField] AnimationCurve chargeVelocityCurve = AnimationCurve.Linear(0, 0, 1, 50);

    [SerializeField] float speedModifier = .6f;

    [Header("aim assist")]
    [SerializeField] AimAssist aimAssist;

    protected CastData lastCastdata;

    bool startedCasting;
    protected override void OnEquip()
    {
        projectileSpawner = new GameObject().AddComponent<ObjectPooler>();
        projectileSpawner.CreatePool(prefab, 10);
    }

    protected override void OnUnEquip(Ability replacement)
    {
        Destroy(projectileSpawner.gameObject);
    }
    protected override void DoCast(CastData data)
    {
		if (!startedCasting)
		{
            caster.ChangeSpeed(-speedModifier);
            startedCasting = true;
        }
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime,0,maxChargeTime);
        lastCastdata = data;
        
        held = true;
        if(chargeTime == maxChargeTime)
        {
            if (releaseOnFullCharge)
			{
                
                held = false;
            }
                
        }

    }

    public override void Tick()
    {
        if(!held && chargeTime > 0)
        {
            
            if(chargeTime >= minChargeTime)
            Launch();

            caster.ChangeSpeed(speedModifier);
            startedCasting =false;
            chargeTime = 0;
        }


        held = false;
    }
    
    protected virtual void Launch()
    {

        float chargeVal = (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime);
        float damage = chargeDamageCurve.Evaluate(chargeVal);
        float speed = chargeVelocityCurve.Evaluate(chargeVal);
        Vector3 velocity = speed * aimAssist.GetAssistedAimDir(lastCastdata.aimDirection,lastCastdata.origin,speed);
        projectileSpawner.Spawn().GetComponent<Projectile>().Launch(lastCastdata.origin,velocity,damage);
        if (OnCast != null)
            OnCast(lastCastdata);
    }

	public override void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(lastCastdata.origin, aimAssist.GetAssistedAimDir(lastCastdata.aimDirection, lastCastdata.origin, 100));
        //Gizmos.DrawWireSphere(caster.transform.position,maxAimAssistRange);
    }
}
