using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    DamageData damage;
    Rigidbody body;
    [SerializeField] Vector3 gravity;
    public bool despawnOnHit = true;
    [SerializeField] Optional<VfxSpawnRequest> vfx;
    public delegate void OnHit();
    public OnHit onHit;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		body.AddForce(gravity,ForceMode.Acceleration);
	}

	public void Launch(Vector3 origin,Vector3 vel,DamageData dmg)
    {
        transform.position = origin;
        body.velocity = vel;
        damage = dmg;
        transform.forward = vel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitBox hb;
        if (collision.collider.TryGetComponent(out hb))
        {
            hb.OnHit(damage);
            if (vfx.Enabled)
                vfx.Value.Play(collision.GetContact(0).point, collision.GetContact(0).normal);
        }
        if(onHit != null)
        onHit();
        if(despawnOnHit)
        GetComponent<PooledObject>().Despawn();
    }
}
