using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Melee")]
public class MeleeAttackAbility : Ability
{
    [SerializeField] float damage;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float swingsPerMin = 1000;
    [SerializeField] float swingAngle;
	[SerializeField] float swingRadius;

	float angleCutoff;
	float coolDown;
	float timer = 0;

	protected override void OnEquip()
	{
		coolDown = 1.0f/ (swingsPerMin / 60.0f);
		angleCutoff = Mathf.Cos(swingAngle / 2);
		Debug.Log(angleCutoff);
		Debug.Log(coolDown);
	}

	public override void Tick()
	{
		timer -= Time.deltaTime;
		
		if (timer < 0)
		{
			timer = 0;
		}
	}


	
	public override void Cast(CastData data)
	{
		if(timer <= 0)
		{
			
			timer = coolDown;
			List<Health> healths = new List<Health>();
			
			Collider[] colliders = Physics.OverlapSphere(data.origin, swingRadius, targetLayers);
			for(int i = 0; i < colliders.Length; i++)
			{
				HitBox hb;
				if(colliders[i].gameObject.TryGetComponent(out hb))
				{
					if (healths.Contains(hb.health))
					continue;
					if(Vector3.Dot(data.aimDirection,(colliders[i].ClosestPoint(data.origin + data.aimDirection * (swingRadius/2) - data.origin)).normalized) > angleCutoff)
					{
						hb.OnHit(damage);

						healths.Add(hb.health);
					}
					
					
					
				}
			}
		}
	}

	public override void OnDrawGizmos()
	{
		
	}
}
