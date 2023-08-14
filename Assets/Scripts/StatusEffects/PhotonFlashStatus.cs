using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEffects/PhotonFlash")]
public class PhotonFlashStatus : StatusEffect
{
	[SerializeField] int ticks = 3;
	[SerializeField] float timeBetweenTicks = .33f;
	[SerializeField] float damage = 10;
	[SerializeField] Optional<VfxSpawnRequest> vfx;
	int tickCompleted = 0;
	float timer = 0;
	protected override void OnAdd()
	{
		tickCompleted = 0;
		timer = 0;
	}

	public override void Tick()
	{
		if(tickCompleted >= ticks)
		{
			health.RemoveStatusEffect(this);
		}
		timer += Time.deltaTime;
		if (timer > timeBetweenTicks)
		{
			health.TakeDmg(damage);
			timer = 0;
			tickCompleted++;
			if (vfx.Enabled)
				vfx.Value.Play(health.transform.position, Vector3.up);
		}
	}
}
