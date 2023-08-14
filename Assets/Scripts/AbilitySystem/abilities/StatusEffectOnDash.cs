using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/DashAddsStatus")]
public class StatusEffectOnDash : DashAbility
{
	[SerializeField] float hitRadius;
	[SerializeField] LayerMask targetLayers;
	List<Health> healths = new List<Health>();
	[SerializeField] StatusEffect effect;
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
			health.AddStatusEffect(effect);
		}
	}
}
