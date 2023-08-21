using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    Rigidbody body;
    [SerializeField] Vector3 gravity;
    [SerializeField] GameObject visual;
    [SerializeField] Optional<VfxSpawnRequest> vfx;
    public delegate void OnHit();
    public OnHit onHit;
    FloatingTextManager textManager;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
        textManager = FindObjectOfType<FloatingTextManager>(); // will replace later
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
        visual.SetActive(true);
        body.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitBox hb;
        if (collision.collider.TryGetComponent(out hb))
        {
            hb.OnHit(damage,this);
            if (vfx.Enabled)
                vfx.Value.Play(collision.GetContact(0).point, collision.GetContact(0).normal);
        }
        if(onHit != null)
        onHit();
        visual.SetActive(false);
        body.isKinematic = true;
        //GetComponent<PooledObject>().Despawn();
    }
}
