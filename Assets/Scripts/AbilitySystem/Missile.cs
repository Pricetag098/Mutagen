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

    void RemoveTarget()
	{
        hitBox.health.OnDeath -= RemoveTarget;
        GetComponent<PooledObject>().Despawn();
    }

	private void Disable()
	{
        hitBox.OnHit(damageData);
        if(hitfx.Enabled)
            hitfx.Value.Play(transform.position, -transform.forward);
        RemoveTarget();
        
	}

	private void Update()
	{
		timer.Tick();

        float t = timer.Progress;
        
        transform.position = QaudraticLerp(origin,mid,target.position,t);

        transform.forward = Vector3.Lerp(mid - origin, target.position - mid,t);


	}

    Vector3 QaudraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
}