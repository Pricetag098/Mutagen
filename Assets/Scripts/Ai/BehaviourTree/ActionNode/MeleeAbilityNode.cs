using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityNode : ActionNode
{
    public int abilityIndex;

    AbilityCaster caster; //blackboard


    protected override void OnStart()
    {
        caster = agent.GetComponent<AbilityCaster>();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        caster.CastAbility(abilityIndex,agent.caster.CreateCastData());


        return State.Success;
    }
}
