using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class MeleeAttackAbility : Ability
{
    [SerializeField] float damage;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float swingsPerMin = 1000;
	[SerializeField] float swingRadius,swingRange;
	[SerializeField] Optional<VfxSpawnRequest> vfx;
	[SerializeField] List<OnHitEffect> hitEffects;
	float angleCutoff;
	float coolDown;
	Timer timer;
	[SerializeField] string animationTrigger;
	protected override void OnEquip()
	{
		coolDown = 1.0f/ (swingsPerMin / 60.0f);
		timer = new Timer(coolDown);
		
	}

	public override void Tick()
	{
		timer.Tick();

	}


	
	protected override void DoCast(CastData data)
	{
		if(timer.complete)
		{
			
            if (OnCast != null)
                OnCast(data);
            timer.Reset();

			if (caster.animator.Enabled)
				caster.animator.Value.SetTrigger(animationTrigger);
			List<Health> healths = new List<Health>();

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
					hb.OnHit(CreateDamageData(damage));
					Vector3 hitPoint = hit.point;
					Vector3 hitNormal = hit.normal;
					if (hitPoint == Vector3.zero)
					{
						hitPoint = hit.collider.ClosestPoint(data.origin);
						hitNormal = data.origin - hitPoint;
					}
					if (vfx.Enabled)
						vfx.Value.Play(hitPoint, hitNormal);
				}
			}
		}
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
