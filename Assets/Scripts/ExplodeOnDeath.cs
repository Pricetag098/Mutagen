using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float radius;
    [SerializeField] DamageData damage;
    [SerializeField] VfxSpawnRequest vfx;

    private void Awake()
    {
        GetComponent<Health>().OnDeath += Explode;
    }
    void Explode()
    {
        vfx.Play(transform.position, Vector3.up);
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
        GetComponent<PooledObject>().Despawn();
    }


}
