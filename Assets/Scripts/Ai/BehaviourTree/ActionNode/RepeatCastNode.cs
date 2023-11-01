using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatCastNode : ActionNode
{
    public int abilityIndex;
    public int repeatCount;
    int castCount;
    public float waitTime;
    public bool assigned;
    public bool rotate;
    [SerializeField] bool deviatedDirection;
    AbilityCaster aCaster;
    float castTime;
    float timer;
    Ability ability;

    bool waiting;
    float waitTimer;

    protected override void OnStart()
    {
        aCaster = agent.caster.caster;
        if (assigned)
        {
            ability = agent.caster.curAbility;
            //return;
        }
        else ability = aCaster.abilities[abilityIndex];


        if (ability.castType == Ability.CastTypes.hold)
        {
            castTime = 1;
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
        if (castCount >= repeatCount)
            return State.Success;

        if (waiting)
        {
            if (Time.time - waitTimer > waitTime)
            {
                waiting = false;
            }
            else return State.Running;
        }


        Ability.CastData data = agent.caster.CreateCastData();
        if (deviatedDirection)
        {
            float dev = Random.Range(-caster.projectileDeviation, caster.projectileDeviation);

            data.aimDirection = new Vector3((player.transform.position.x - agent.transform.position.x)
                + dev, 0, (player.transform.position.z - agent.transform.position.z) + dev).normalized;

        }

        if (ability.GetType() == typeof(DashAbility) || ability.GetType() == typeof(DashApplysEffect))
        {
            data.moveDirection = (blackboard.moveToPosition - agent.transform.position).normalized * 10;
        }

        if (rotate)
            agent.transform.LookAt(blackboard.rotateTowardsObject.transform.position);

        if (!assigned)
            aCaster.CastAbility(abilityIndex, data);
        else
        {
            for (int i = 0; i < aCaster.abilities.Length; i++)
            {
                if (aCaster.abilities[i] == ability)
                {
                    aCaster.CastAbility(i, data);
                }
            }
        }

        if (ability.castType == Ability.CastTypes.hold)
        {
            timer += Time.fixedDeltaTime;
            if (timer > castTime)
            {
                timer = 0;
                if (agent.retaliate) agent.retaliate = false;
                castCount++;
                waiting = true;
                waitTimer = Time.time;
                return State.Running;
            }

            else
            {
                waiting = true;
                waitTimer = Time.time;
                return State.Running;
            }
        }
        if (agent.retaliate) agent.retaliate = false;
        castCount++;
        waiting = true;
        waitTimer = Time.time;
        return State.Running;
    }
}
