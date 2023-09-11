using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitAura : MonoBehaviour
{
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float radius;
    [SerializeField] DamageData damage;
    [SerializeField] float tickTime;
    Timer timer;
    [SerializeField] List<OnHitEffect> onHitEffects;
    private void Awake()
    {
        //GetComponent<Projectile>().onHit += Explode;
        timer = new Timer(tickTime);
    }
    private void Update()
    {
        timer.Tick();
        if (!timer.complete)
            return;

        timer.Reset();
        List<Health> healths = new List<Health>();
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, targetLayers);
        foreach (Collider hit in hits)
        {
            HitBox hb;
            if (hit.GetComponent<Collider>().TryGetComponent(out hb))
            {
                if (healths.Contains(hb.health))
                {
                    continue;
                }
                healths.Add(hb.health);
                Vector3 dir = (transform.position - hb.health.transform.position).normalized;
                foreach(OnHitEffect onHitEffect in onHitEffects)
                {
                    onHitEffect.OnHit(hb,dir);
                }
                hb.OnHit(damage);
            }
        }
    }
}
