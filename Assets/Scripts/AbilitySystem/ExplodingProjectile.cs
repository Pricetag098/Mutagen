using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Projectile))]
public class ExplodingProjectile : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
	[SerializeField] float radius;
	[SerializeField] float damage;
	[SerializeField] ParticleSystem particleSystem;

	private void Awake()
	{
		GetComponent<Projectile>().onHit += Explode;
	}
	void Explode()
	{
		particleSystem.Play();
		List<Health> healths = new List<Health>();

		Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers);
		foreach (Collider hit in hits)
		{
			HitBox hb;
			if (hit.GetComponent<Collider>().TryGetComponent(out hb))
			{
				hb.OnHit(damage);
			}
		}
		
	}
}
