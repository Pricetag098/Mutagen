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
    [SerializeField] List<StatusEffect> statusEffects;
    private void Awake()
    {
        //GetComponent<Projectile>().onHit += Explode;
        timer = new Timer(tickTime);
    }

    private void OnEnable()
    {
        timer.Reset();
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
                if (healths.Contains(hb.health) || hb.health.gameObject == gameObject)
                {
                    continue;
                }
                healths.Add(hb.health);
                Vector3 dir = (hb.health.transform.position-transform.position);
                foreach(OnHitEffect onHitEffect in onHitEffects)
                {
                    onHitEffect.OnHit(hb,dir);
                }
                foreach(StatusEffect statusEffect in statusEffects)
                {
                    hb.health.AddStatusEffect(statusEffect);
                }
                hb.OnHit(damage);
            }
        }
    }
}
