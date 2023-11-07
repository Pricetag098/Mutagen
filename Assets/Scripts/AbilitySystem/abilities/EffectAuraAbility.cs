using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Aura")]
public class EffectAuraAbility : Ability
{
    [SerializeField] Optional<VfxSpawnRequest> vfx;
    [SerializeField] float radius;
    [SerializeField] float damage;
    [SerializeField] List<StatusEffect> statusEffects;
    [SerializeField] List<OnHitEffect> onHitEffects;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float cooldown;
    [SerializeField] float offset;
    [SerializeField] string animationTrigger;
    Timer timer;

    protected override void OnEquip()
    {
        timer = new Timer(cooldown);
    }
    protected override void DoCast(CastData data)
    {
        if (!timer.complete)
            return;
        FinishCast();
        data.origin = caster.transform.position;
        data.origin.y += offset;

        timer.Reset();
        if(vfx.Enabled)
        vfx.Value.Play(data.origin, data.aimDirection);
        List<Health> healths = new List<Health>();

        Collider[] hits = Physics.OverlapSphere(data.origin, radius, layerMask);
        foreach (Collider hit in hits)
        {
            HitBox hb;
            if (hit.GetComponent<Collider>().TryGetComponent(out hb))
            {
                hb.OnHit(CreateDamageData(damage));
                foreach(OnHitEffect onHitEffect in onHitEffects)
                {
                    onHitEffect.OnHit(hb,hb.health.transform.position - data.origin);
                }
                foreach(StatusEffect statusEffect in statusEffects)
                {
                    hb.health.AddStatusEffect(statusEffect);
                }

                if (caster.animator.Enabled)
                    caster.animator.Value.SetTrigger(animationTrigger);
            }
        }
    }
    public override void OnTick()
    {
        timer.Tick();
    }

    public override float GetCoolDownPercent()
    {
        return timer.Progress;
    }

}
