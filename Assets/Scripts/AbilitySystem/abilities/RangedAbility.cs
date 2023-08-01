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

    protected Vector3 lastAimDir,lastOrigin;
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
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime,0,maxChargeTime);
        lastAimDir = data.aimDirection;
        lastOrigin = data.origin;
        held = true;
    }

    public override void Tick()
    {
        if(!held && chargeTime > 0)
        {
            Debug.Log("Relese");
            if(chargeTime > minChargeTime)
            Launch();
            


            chargeTime = 0;
        }


        held = false;
    }

    protected virtual void Launch()
    {
        float damage = chargeDamageCurve.Evaluate(chargeTime / maxChargeTime);
        Vector3 velocity = chargeVelocityCurve.Evaluate(chargeTime / maxChargeTime) * lastAimDir;
        projectileSpawner.Spawn().GetComponent<Projectile>().Launch(lastOrigin,velocity,damage);
    }
}
