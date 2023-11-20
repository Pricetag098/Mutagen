using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class MeleeAttackAbility : Ability
{
    [SerializeField] float damage;
	[SerializeField] int comboLength;
	int comboIndex = 0;
	bool inputBuffered = false;
	[SerializeField] float damageRange;
    [SerializeField] LayerMask targetLayers;
    //[SerializeField] float swingsPerMin = 1000;
	[SerializeField] float swingRadius,swingRange;
	[SerializeField] Optional<VfxSpawnRequest> hitvfx;
	[SerializeField] Optional<VfxSpawnRequest> swingvfx;
	[SerializeField] protected List<OnHitEffect> hitEffects;
	[SerializeField,Range(0,1)] float bufferWindow;
	float angleCutoff;
	[SerializeField]float coolDown;
	Timer timer;
	[SerializeField] string animationTrigger;
	
	

	protected override void OnEquip()
	{
		//coolDown = 1.0f/ (swingsPerMin / 60.0f);
		//caster.ChangeSpeed(-speedModifier);

        timer = new Timer(coolDown,true);
	}

	public override void OnTick()
	{
		timer.Tick();
		if (timer.complete && castState == CastState.casting)
		{
			if (inputBuffered)
			{
				inputBuffered = false;
				if (OnCast != null)
					OnCast(lastCastData);
				timer.Reset();
				OnSwing(lastCastData.origin, lastCastData.aimDirection);
				TriggerAnimation(animationTrigger, coolDown);
				List<Health> healths = new List<Health>();
				if (swingvfx.Enabled)
					swingvfx.Value.Play(lastCastData.origin, lastCastData.moveDirection, lastCastData.effectOrigin);
				RaycastHit[] hits = Physics.SphereCastAll(lastCastData.origin, swingRadius, lastCastData.moveDirection, swingRange, targetLayers);
				foreach (RaycastHit hit in hits)
				{
					HitBox hb;
					if (hit.collider.TryGetComponent(out hb))
					{
						if (healths.Contains(hb.health))
							continue;
						healths.Add(hb.health);
						OnHit(hb, lastCastData.aimDirection);

						float tempdmg = damage;
						tempdmg += Random.Range(-damageRange, damageRange);
						if (tempdmg < 0)
							tempdmg = 0;

						hb.OnHit(CreateDamageData(tempdmg));
						Vector3 hitPoint = hit.point;
						Vector3 hitNormal = hit.normal;
						if (hitPoint == Vector3.zero)
						{
							hitPoint = hit.collider.ClosestPoint(lastCastData.origin);
							hitNormal = lastCastData.origin - hitPoint;
						}
						if (hitvfx.Enabled)
							hitvfx.Value.Play(hitPoint, hitNormal);
					}
				}
				comboIndex++;
			}
			else
			{
				FinishCast();
				comboIndex = 0;
			}

		}
		




		
	}
	protected override void OnUnEquip(Ability replacement)
	{
		
	}
	protected override void DoCast(CastData data)
	{
		if(!inputBuffered&&timer.Progress > bufferWindow && comboIndex < comboLength)
		{
			inputBuffered = true;
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
