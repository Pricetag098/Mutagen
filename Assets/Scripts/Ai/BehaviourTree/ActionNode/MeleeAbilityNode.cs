using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbilityNode : ActionNode
{
    public int abilityIndex;
    EnemyAbilityCaster aiCaster; //blackboard
    AbilityCaster caster; //blackboard


    protected override void OnStart()
    {
        aiCaster = agent.GetComponent<EnemyAbilityCaster>();
        caster = agent.GetComponent<AbilityCaster>();
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        caster.CastAbility(abilityIndex,aiCaster.CreateCastData());
        agent.performingAction = true;
        agent.actionTimer = Time.time;


        return State.Success;
    }
}
