using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    Rigidbody body;
    [SerializeField] Vector3 gravity;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		body.AddForce(gravity,ForceMode.Acceleration);
	}

	public void Launch(Vector3 origin,Vector3 vel,float dmg)
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
        }
        GetComponent<PooledObject>().Despawn();
    }
}
