using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityNode : ActionNode
{
    public int abilityIndex;
    AbilityCaster aCaster;

    protected override void OnStart()
    {
        aCaster = agent.caster.caster;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        aCaster.CastAbility(abilityIndex,agent.caster.CreateCastData());
        agent.performingAction = true;
        agent.actionTimer = Time.time; 
        return State.Success;
    }
}
