using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="StatusEffects/SkipStrike")]
public class SkipStrikeStatus : StatusEffect
{
	float totalDamage = 0;
	[SerializeField] float duration;
	[SerializeField] float reflectedDamageMulti = 1;
	[SerializeField] Optional<VfxSpawnRequest> vfx;
	Timer timer;
	protected override void OnAdd()
	{
		health.OnHit += OnHit;
		timer = new Timer(duration,false);
	}

	public override void Tick()
	{
		timer.Tick();
		if(timer.complete)
		{
			
			health.TakeDmg(totalDamage * reflectedDamageMulti);
			if (vfx.Enabled)
				vfx.Value.Play(health.transform.position, Vector3.up);
			health.RemoveStatusEffect(this);
		}
	}

	protected override void OnRemove()
	{
		health.OnHit -= OnHit;
	}

	void OnHit(float dmg)
	{
		totalDamage += dmg;
	}
}
