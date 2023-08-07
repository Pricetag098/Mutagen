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

    protected Vector3 lastAimDir,lastOrigin;

    bool startedCasting;
    protected override void OnEquip()
    {
        projectileSpawner = Instantiate(prefab).GetComponent<ObjectPooler>();
    }

    protected override void OnUnEquip()
    {
        Destroy(projectileSpawner.gameObject);
    }
    public override void Cast(CastData data)
    {
		if (!startedCasting)
		{
            caster.ChangeSpeed(-speedModifier);
            startedCasting = true;
        }
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime,0,maxChargeTime);
        lastAimDir = data.aimDirection;
        lastOrigin = data.origin;
        
        held = true;
        if(chargeTime == maxChargeTime)
        {
            if (releaseOnFullCharge)
                Launch();
        }

    }

    public override void Tick()
    {
        if(!held && chargeTime > 0)
        {
            
            if(chargeTime > minChargeTime)
            Launch();

            caster.ChangeSpeed(speedModifier);
            startedCasting =false;
            chargeTime = 0;
        }


        held = false;
    }
    
    protected virtual void Launch()
    {
        float damage = chargeDamageCurve.Evaluate(chargeTime / maxChargeTime);
        float speed = chargeVelocityCurve.Evaluate(chargeTime / maxChargeTime);
        Vector3 velocity = speed * aimAssist.GetAssistedAimDir(lastAimDir,lastOrigin,speed);
        projectileSpawner.Spawn().GetComponent<Projectile>().Launch(lastOrigin,velocity,damage);
    }

	public override void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(lastOrigin, aimAssist.GetAssistedAimDir(lastAimDir, lastOrigin, 100));
        //Gizmos.DrawWireSphere(caster.transform.position,maxAimAssistRange);
    }
}
