using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CastAbilityNode : ActionNode
{
    public int abilityIndex;
    public float tempCastTime;
    public bool assigned;
    public bool rotate;
    AbilityCaster aCaster;
    float castTime;
    float timer;
    Ability ability;

    protected override void OnStart()
    {
        aCaster = agent.caster.caster;
        if (assigned)
        {
            ability = agent.caster.curAbility;
            return;
        }

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

        if(ability.GetType() == typeof (DashAbility) || ability.GetType() == typeof(DashApplysEffect))
        {
            data.moveDirection = (blackboard.moveToPosition - agent.transform.position).normalized * 10;
        }

        if(rotate)
            agent.transform.LookAt(blackboard.rotateTowardsObject.transform.position);

        if (!assigned)
        aCaster.CastAbility(abilityIndex,data);
        else
        {
            for(int i = 0; i < aCaster.abilities.Length; i++)
            {
                if(aCaster.abilities[i] == ability)
                {
                    aCaster.CastAbility(i,data);
                }
            }
        }

        agent.performingAction = true;
        agent.actionTimer = Time.time;

        if (ability.castType == Ability.CastTypes.hold)
        {
            timer += Time.deltaTime;
            if (timer > castTime)
            {
                timer = 0;
                if (agent.retaliate) agent.retaliate = false;
                return State.Success;
            }

            else return State.Running;
        }
        if(agent.retaliate) agent.retaliate = false;
        return State.Success;
    }
}
