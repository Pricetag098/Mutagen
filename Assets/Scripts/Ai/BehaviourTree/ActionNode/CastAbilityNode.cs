using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastAbilityNode : ActionNode
{
    public int abilityIndex;
    AbilityCaster aCaster;
    public float castTime;
    float timer;

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
        timer += Time.deltaTime;
        if (timer > castTime)
        {
            timer = 0;
            return State.Success;
        }

        else return State.Running;


    }
}
