using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="StatusEffects/SkipStrike")]
public class SkipStrikeStatus : StatusEffect
{
	float totalDamage = 0;
	public float duration;
	public float reflectedDamageMulti = 1;
	[SerializeField] Optional<VfxSpawnRequest> vfx;
	public Timer timer;
	[SerializeField] Ability.CastTypes type;
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
			
			health.TakeDmg(CreateDamageData(totalDamage * reflectedDamageMulti));
			if (vfx.Enabled)
				vfx.Value.Play(health.transform.position, Vector3.up);
			health.RemoveStatusEffect(this);
		}
	}

	protected override void OnRemove()
	{
		health.OnHit -= OnHit;
	}

	void OnHit(DamageData data)
	{
		totalDamage += data.damage;
	}

	public override void Combine(StatusEffect effect)
	{
		SkipStrikeStatus status = effect as SkipStrikeStatus;
		reflectedDamageMulti += status.reflectedDamageMulti;
	}

    protected virtual DamageData CreateDamageData(float damage)
    {
        DamageData data = new DamageData();
        data.damage = damage;
        data.type = type;
        return data;
    }
}
