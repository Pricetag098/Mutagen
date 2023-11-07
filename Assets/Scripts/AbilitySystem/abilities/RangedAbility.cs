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
    public float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;

    [SerializeField] float damageRange;
    [SerializeField] AnimationCurve chargeDamageCurve = AnimationCurve.Linear(0,0,1,100);
    [SerializeField] AnimationCurve chargeVelocityCurve = AnimationCurve.Linear(0, 0, 1, 50);

    [SerializeField] float speedModifier = .6f;

    [SerializeField] List<OnHitEffect> onHitEffectList = new List<OnHitEffect>();   

    [Header("aim assist")]
    [SerializeField] AimAssist aimAssist;

    protected CastData lastCastdata;

    bool startedCasting;
    [SerializeField] Optional<VfxSpawnRequest> vfx;

    [SerializeField] string animationTrigger;
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

    public override void OnTick()
    {
        if(!held && chargeTime > 0)
        {
            
            if(chargeTime >= minChargeTime)
            Launch();

            
            startedCasting =false;
            chargeTime = 0;
        }


        held = false;
    }
    
    protected virtual void Launch()
    {
        if(vfx.Enabled)
            vfx.Value.Play(lastCastdata.origin, lastCastdata.aimDirection);
        float chargeVal = (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime);
        float damage = chargeDamageCurve.Evaluate(chargeVal);
        float tempdmg = damage;
        tempdmg += Random.Range(-damageRange, damageRange);
        if (tempdmg < 0)
            tempdmg = 0;
        float speed = chargeVelocityCurve.Evaluate(chargeVal);
        Vector3 velocity = speed * aimAssist.GetAssistedAimDir(lastCastdata.aimDirection,lastCastdata.origin,speed);
        projectileSpawner.Spawn().GetComponent<Projectile>().Launch(lastCastdata.origin,velocity,CreateDamageData(tempdmg),onHitEffectList);
        if (OnCast != null)
            OnCast(lastCastdata);

        if (caster.animator.Enabled)
            caster.animator.Value.SetTrigger(animationTrigger);
        FinishCast();
    }

	public override void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(lastCastdata.origin, aimAssist.GetAssistedAimDir(lastCastdata.aimDirection, lastCastdata.origin, 100));
        //Gizmos.DrawWireSphere(caster.transform.position,maxAimAssistRange);
    }
}
