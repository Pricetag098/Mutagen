using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Missiles")]
public class MissilesAbility : Ability
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] bool releaseOnFullCharge;
    protected ObjectPooler projectileSpawner;

    [SerializeField] float damage = 10;
    [SerializeField] float range = 100;
    [SerializeField] float spreadRad = 10;
    [SerializeField] float upOffset = 3;
    [SerializeField] float projectileSpeed = 30;
    [SerializeField] LayerMask targetLayers;
    protected float chargeTime;
    [SerializeField] protected float maxChargeTime;
    [SerializeField] protected float minChargeTime;
    bool held;

    [SerializeField] AnimationCurve launchCount = AnimationCurve.Linear(0,3,1,15);

    
    [SerializeField] float speedModifier = .6f;

    [Header("aim assist")]
    [SerializeField] AimAssist aimAssist;

    protected CastData lastCastdata;

    bool startedCasting;
    protected override void OnEquip()
    {
        projectileSpawner = new GameObject().AddComponent<ObjectPooler>();
        projectileSpawner.CreatePool(prefab, 5 * (int)launchCount.keys[launchCount.keys.Length -1].value);
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
        chargeTime = Mathf.Clamp(chargeTime + Time.deltaTime, 0, maxChargeTime);
        lastCastdata = data;

        held = true;
        if (chargeTime == maxChargeTime)
        {
            if (releaseOnFullCharge)
            {

                held = false;
            }

        }

    }

    public override void Tick()
    {
        if (!held && chargeTime > 0)
        {

            if (chargeTime >= minChargeTime)
                Launch();

            caster.ChangeSpeed(speedModifier);
            startedCasting = false;
            chargeTime = 0;
        }


        held = false;
    }

    protected virtual void Launch()
    {
        RaycastHit hit;

        Vector3 aimDir = aimAssist.GetAssistedAimDir(lastCastdata.aimDirection, lastCastdata.origin, float.PositiveInfinity);
        if (Physics.Raycast(lastCastdata.origin,aimDir,out hit, range, targetLayers))
		{
            HitBox hb;
            if(hit.transform.TryGetComponent(out hb))
			{

                Vector3 rightDir = Vector3.Cross(aimDir, Vector3.up);
                Vector3 upDir = -Vector3.Cross(aimDir, rightDir);
                float chargeVal = (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime);
                for (int i = 0; i < Mathf.RoundToInt(launchCount.Evaluate(chargeVal)); i++)
                {
                    Vector2 circlePos = Random.insideUnitCircle * spreadRad;
                    circlePos.y = Mathf.Abs(circlePos.y);

                    Vector3 midPoint = Vector3.Lerp(lastCastdata.origin, hb.transform.position, .5f) + upDir * circlePos.y + rightDir * circlePos.x;

                    projectileSpawner.Spawn().GetComponent<Missile>().Launch(lastCastdata.origin, midPoint, hb.transform, CreateDamageData(damage),projectileSpeed,hb);
                }
            }
		}
        
        
        
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
