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

	float angleCutoff;
	float coolDown;
	float timer = 0;

	protected override void OnEquip()
	{
		coolDown = 1.0f/ (swingsPerMin / 60.0f);

	}

	public override void Tick()
	{
		timer -= Time.deltaTime;
		
		if (timer < 0)
		{
			timer = 0;
		}
	}


	
	protected override void DoCast(CastData data)
	{
		if(timer <= 0)
		{
            if (OnCast != null)
                OnCast(data);
            timer = coolDown;
			List<Health> healths = new List<Health>();

			RaycastHit[] hits = Physics.SphereCastAll(data.origin, swingRadius, data.aimDirection, swingRange, targetLayers);
			foreach (RaycastHit hit in hits)
			{
				HitBox hb;
				if (hit.collider.TryGetComponent(out hb))
				{
					hb.OnHit(damage);
				}
			}
		}
	}

	public override void OnDrawGizmos()
	{
		
	}
}
