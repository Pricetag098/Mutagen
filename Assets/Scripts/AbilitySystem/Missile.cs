using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Transform target;
    Vector3 origin;
    Vector3 mid;
    HitBox hitBox;
    Timer timer;
    DamageData damageData;
    [SerializeField] Optional<VfxSpawnRequest> hitfx;
    [SerializeField, Range(0, 1)] float targetingPoint = .5f;
    [SerializeField] float randRadius;
    [SerializeField] LayerMask layer;
    [SerializeField] float hitRadius;
    Vector3 rand;
    public void Launch(Vector3 origin,Vector3 mid,Transform target,DamageData damage,float speed,HitBox hitBox)
	{
        this.origin = origin;
        this.mid = mid;
        this.target = target;
        this.damageData = damage;
        this.hitBox = hitBox;

        hitBox.health.OnDeath += RemoveTarget;

        float aproxDist = Vector3.Distance(origin, mid) + Vector3.Distance(mid,target.position);
        timer = new Timer(aproxDist/speed,false,Disable);
	}

    void RemoveTarget(DamageData data)
	{
        hitBox.health.OnDeath -= RemoveTarget;
        GetComponent<PooledObject>().Despawn();
    }

	private void Disable()
	{
        Collider[] colliders = Physics.OverlapSphere(transform.position,hitRadius,layer);
        List<Health> healths = new List<Health>();
        foreach(Collider collider in colliders)
        {
            if(collider.TryGetComponent(out HitBox hitbox))
            {
                if (!healths.Contains(hitBox.health))
                {
                    healths.Add(hitBox.health);
                    hitbox.OnHit(damageData);
                }
                
            }
        }
        if(hitfx.Enabled)
            hitfx.Value.Play(transform.position, -transform.forward);
        RemoveTarget(new DamageData());
        
	}
    Vector3 targetPos;
    private void Update()
	{
		timer.Tick();

        float t = timer.Progress;
        if(t < targetingPoint)
        {
            targetPos = target.position;
        }
        
        
        transform.position = QaudraticLerp(origin,mid,targetPos,t);

        transform.forward = Vector3.Lerp(mid - origin, targetPos - mid,t);


	}

    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
}
