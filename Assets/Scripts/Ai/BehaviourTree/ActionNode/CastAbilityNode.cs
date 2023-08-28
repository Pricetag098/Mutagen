using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastAbilityNode : ActionNode
{
    public int abilityIndex;
    public float tempCastTime;
    AbilityCaster aCaster;
    float castTime;
    float timer;
    Ability ability;

    protected override void OnStart()
    {
        aCaster = agent.caster.caster;
        ability = aCaster.abilities[abilityIndex];
        if (ability.castType == Ability.CastTypes.hold)
        {
            castTime = tempCastTime;
        }

        if (ability.GetType() == typeof(RangedAbility))
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
        Ability.CastData data = agent.caster.CreateCastData();
        if(ability.GetType() == typeof(DashAbility))
        {
            data.moveDirection = blackboard.moveToPosition;
        }


        aCaster.CastAbility(abilityIndex,data);
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
