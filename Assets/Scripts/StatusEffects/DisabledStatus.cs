using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StatusEffects/Disabled")]
public class DisabledStatus : StatusEffect
{
    public float duration;

    Timer timer;
    protected override void OnAdd()
    {
        timer = new Timer(duration, false);
        if(health.TryGetComponent(out AbilityCaster abilityCaster))
        {
            abilityCaster.castBool = false;
        }
        
    }

    public override void Tick()
    {
        if(timer.complete)
            health.RemoveStatusEffect(this);        
    }
    public override void Combine(StatusEffect effect)
    {
        DisabledStatus status = effect as DisabledStatus;
        timer.maxTime += status.duration;
    }
    protected override void OnRemove()
    {
        if (health.TryGetComponent(out AbilityCaster abilityCaster))
        {
            abilityCaster.castBool = true;
        }
    }
}
