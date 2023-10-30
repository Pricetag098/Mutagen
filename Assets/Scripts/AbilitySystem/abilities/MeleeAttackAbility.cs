using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class MeleeAttackAbility : Ability
{
    [SerializeField] float damage;
	[SerializeField] float damageRange;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float swingsPerMin = 1000;
	[SerializeField] float swingRadius,swingRange;
	[SerializeField] Optional<VfxSpawnRequest> hitvfx;
	[SerializeField] Optional<VfxSpawnRequest> swingvfx;
	[SerializeField] protected List<OnHitEffect> hitEffects;
	[SerializeField] float speedModifier;
	float angleCutoff;
	float coolDown;
	Timer timer;
	[SerializeField] string animationTrigger;
	bool swung = false;
	protected override void OnEquip()
	{
		coolDown = 1.0f/ (swingsPerMin / 60.0f);
		caster.ChangeSpeed(-speedModifier);

        timer = new Timer(coolDown,true);
	}

	public override void Tick()
	{
		timer.Tick();
		if(timer.complete && swung)
        {
			caster.ChangeSpeed(speedModifier);
			swung = false;
        }
	}
	
	protected override void DoCast(CastData data)
	{
		if(timer.complete)
		{
			swung = true;
			caster.ChangeSpeed(-speedModifier);
            if (OnCast != null)
                OnCast(data);
            timer.Reset();
			OnSwing(data.origin, data.aimDirection);
			if (caster.animator.Enabled)
				caster.animator.Value.SetTrigger(animationTrigger);
			List<Health> healths = new List<Health>();
			if (swingvfx.Enabled)
				swingvfx.Value.Play(data.origin, data.aimDirection,data.effectOrigin);
			RaycastHit[] hits = Physics.SphereCastAll(data.origin, swingRadius, data.aimDirection, swingRange, targetLayers);
			foreach (RaycastHit hit in hits)
			{
				HitBox hb;
				if (hit.collider.TryGetComponent(out hb))
				{
					if (healths.Contains(hb.health))
						continue;
					healths.Add(hb.health);
					OnHit(hb,data.aimDirection);

                    damage += Random.Range(-damageRange, damageRange);
                    if (damage < 0)
                        damage = 0;

                    hb.OnHit(CreateDamageData(damage));
					Vector3 hitPoint = hit.point;
					Vector3 hitNormal = hit.normal;
					if (hitPoint == Vector3.zero)
					{
						hitPoint = hit.collider.ClosestPoint(data.origin);
						hitNormal = data.origin - hitPoint;
					}
					if (hitvfx.Enabled)
						hitvfx.Value.Play(hitPoint, hitNormal);
				}
			}
		}
	}
	
	
	protected virtual void OnSwing(Vector3 origin,Vector3 direction)
	{

	}

	public override void OnDrawGizmos()
	{
		
	}
	protected virtual void OnHit(HitBox hb,Vector3 direction)
	{
		foreach(OnHitEffect effect in hitEffects)
		{
			effect.OnHit(hb, direction);
		}
	}

	public override float GetCoolDownPercent()
	{
		return timer.Progress;
	}
}
