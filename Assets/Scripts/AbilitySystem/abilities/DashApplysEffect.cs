using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DashAddsStatus")]
public class DashApplysEffect : DashAbility
{
	[SerializeField] float hitRadius;
	[SerializeField] LayerMask targetLayers;
	List<Health> healths = new List<Health>();
	[SerializeField] List<StatusEffect> statusEffects;
	[SerializeField] List<OnHitEffect> onHitEffects;
	protected override void WhileDashing()
	{
		base.WhileDashing();

		Collider[] colliders = Physics.OverlapSphere(caster.transform.position, hitRadius,targetLayers);
		foreach (Collider collider in colliders)
		{
			HitBox hb;
			if(collider.TryGetComponent(out hb))
			{
				if (!healths.Contains(hb.health))
				{
					healths.Add(hb.health);
					foreach(OnHitEffect onHitEffect in onHitEffects)
					{
						onHitEffect.OnHit(hb, direction);
					}
				}
			}
		}
	}
	protected override void DoDash()
	{
		base.DoDash();
		healths.Clear();
	}

	protected override void EndDash()
	{
		base.EndDash();
		foreach(Health health in healths)
		{
			foreach(StatusEffect statusEffect in statusEffects)
			{
                health.AddStatusEffect(statusEffect);
            }
			
		}
	}
}
