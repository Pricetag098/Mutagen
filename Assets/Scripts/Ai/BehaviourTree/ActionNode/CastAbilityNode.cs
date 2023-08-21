using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastAbilityNode : ActionNode
{
    public int abilityIndex;
    AbilityCaster aCaster;
    float castTime;
    float timer;
    Ability ability;

    protected override void OnStart()
    {
        aCaster = agent.caster.caster;
        ability = aCaster.abilities[abilityIndex];
        if (ability.GetType() == typeof (RangedAbility))
        {
            RangedAbility rAbility = ability as RangedAbility;
            castTime = rAbility.maxChargeTime;
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        aCaster.CastAbility(abilityIndex,agent.caster.CreateCastData());
        agent.performingAction = true;
        agent.actionTimer = Time.time;

        if (ability.castType == Ability.CastTypes.hold)
        {
            timer += Time.deltaTime;
            if (timer > castTime)
            {
                timer = 0;
                return State.Success;
            }

            else return State.Running;
        }
        return State.Success;
    }
}
