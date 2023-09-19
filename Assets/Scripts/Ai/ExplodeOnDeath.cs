using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float radius;
    [SerializeField] DamageData damage;

    [SerializeField] bool explodeOnAnyElement = true;
    [SerializeField] VfxSpawnRequest vfx;
    [SerializeField] DamageData damage2;
    [SerializeField] VfxSpawnRequest vfx2;
    [SerializeField] Element element;

    private void Awake()
    {
        GetComponent<Health>().OnDeath += Explode;
    }

    void Explode(DamageData data)
    {

        if(data.type == element)
        {
            vfx2.Play(transform.position, Vector3.up);
            List<Health> healths = new List<Health>();
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers);
            foreach (Collider hit in hits)
            {
                HitBox hb;
                if (hit.GetComponent<Collider>().TryGetComponent(out hb))
                {
                    hb.OnHit(damage2);
                }
            }
            GetComponent<PooledObject>().Despawn();
        }
        else
        {
            if (!explodeOnAnyElement)
                return;
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


}
