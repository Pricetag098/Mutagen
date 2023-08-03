using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ranged")]
public class RangedAbility : Ability
{
    [SerializeField] protected GameObject prefab;
    protected ObjectPooler projectileSpawner;

    protected float chargeTime;
    [SerializeField]protected float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;

    [SerializeField] AnimationCurve chargeDamageCurve = AnimationCurve.Linear(0,0,1,100);
    [SerializeField] AnimationCurve chargeVelocityCurve = AnimationCurve.Linear(0, 0, 1, 50);

    [SerializeField] float speedModifier = .6f;

    [Header("aim assist")]
    [SerializeField] float maxAimAssistRange = 100;
    [SerializeField] LayerMask aimAssistLayer;
    [SerializeField] float aimAssistDirectionWeight = 1;
    [SerializeField] float aimAssistDistanceWeight = 1;
    [SerializeField] float aimAssistPriorityWeight = 1;
    [Range(-1,1)]
    [SerializeField] float aimAssistMinDirectionValue = -1;
    [Range(0f, 1f)]  
    [SerializeField] float assistWeight = 1;

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
        Vector3 velocity = chargeVelocityCurve.Evaluate(chargeTime / maxChargeTime) * GetAssistedAimDir(lastAimDir,aimAssistLayer,maxAimAssistRange,aimAssistMinDirectionValue,aimAssistDirectionWeight,aimAssistDistanceWeight,aimAssistPriorityWeight,assistWeight);
        projectileSpawner.Spawn().GetComponent<Projectile>().Launch(lastOrigin,velocity,damage);
    }

	public override void OnDrawGizmos()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(lastOrigin,GetAssistedAimDir(lastAimDir, aimAssistLayer, maxAimAssistRange, aimAssistMinDirectionValue, aimAssistDirectionWeight, aimAssistDistanceWeight, aimAssistPriorityWeight, assistWeight));
        Gizmos.DrawWireSphere(caster.transform.position,maxAimAssistRange);
    }
}
