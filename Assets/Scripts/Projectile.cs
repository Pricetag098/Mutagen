using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage;
    Rigidbody body;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 origin,Vector3 vel,float dmg)
    {
        transform.position = origin;
        body.velocity = vel;
        damage = dmg;

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
