using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="StatusEffects/SkipStrike")]
public class SkipStrikeStatus : StatusEffect
{
	float totalDamage = 0;
	[SerializeField] float duration;
	[SerializeField] float reflectedDamageMulti = 1;
	float timer;
	protected override void OnAdd()
	{
		health.OnHit += OnHit;
		timer = 0;
	}

	public override void Tick()
	{
		timer += Time.deltaTime;
		if(timer > duration)
		{
			
			health.TakeDmg(totalDamage * reflectedDamageMulti);
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
