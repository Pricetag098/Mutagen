using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEffects/PhotonFlash")]
public class PhotonFlashStatus : StatusEffect
{
	public int ticks = 3;
	public float timeBetweenTicks = .33f;
	public float damage = 10;
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

	public override void Combine(StatusEffect effect)
	{
		PhotonFlashStatus status = effect as PhotonFlashStatus;
		ticks += status.ticks;
		timeBetweenTicks = Mathf.Min(timeBetweenTicks, status.timeBetweenTicks);
	}
}
