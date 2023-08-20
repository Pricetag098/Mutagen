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
	Timer timer;
	protected override void OnAdd()
	{
		tickCompleted = 0;
		timer = new Timer(timeBetweenTicks,false);
	}

	public override void Tick()
	{
		if(tickCompleted >= ticks)
		{
			health.RemoveStatusEffect(this);
		}
		timer.Tick();
		if (timer.complete)
		{
			health.TakeDmg(damage);
			timer.Reset();
			tickCompleted++;
			if (vfx.Enabled)
				vfx.Value.Play(health.transform.position, Vector3.up);
		}
	}
}
