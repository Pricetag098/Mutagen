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
    public float lifeTime = float.PositiveInfinity;
    Timer timer;
    List<OnHitEffect> onHitEffects;
    // Start is called before the first frame update
    void Awake()
    {
        timer = new Timer(lifeTime,false);
        body = GetComponent<Rigidbody>();
    }

	private void FixedUpdate()
	{
		body.AddForce(gravity,ForceMode.Acceleration);
	}
    private void Update()
    {
        timer.Tick();
        if(timer.complete)
            GetComponent<PooledObject>().Despawn();
    }
    public void Launch(Vector3 origin,Vector3 vel,DamageData dmg)
    {
        timer.Reset();
        transform.position = origin;
        body.velocity = vel;
        damage = dmg;
        transform.forward = vel;
    }

    public void Launch(Vector3 origin, Vector3 vel, DamageData dmg,List<OnHitEffect> onhits)
    {
        timer.Reset();
        transform.position = origin;
        body.velocity = vel;
        damage = dmg;
        transform.forward = vel;
        onHitEffects = onhits;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitBox hb;
        if (collision.collider.TryGetComponent(out hb))
        {
            hb.OnHit(damage);
            if (vfx.Enabled)
                vfx.Value.Play(collision.GetContact(0).point, collision.GetContact(0).normal);
            foreach (OnHitEffect onHitEffect in onHitEffects)
            {
                onHitEffect.OnHit(hb, transform.forward);
            }
        }
        
        if(onHit != null)
        onHit();
        if(despawnOnHit)
        GetComponent<PooledObject>().Despawn();
    }
}
