using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CheckType
{
    DistanceLessThan,
    DistanceGreaterThan,
    Health,
    isStunned,
    retaliate,
    delayMove,
    groupDistance,
    groupFacing,
    flanking,
    sightLine,
    abilityType,
    isAbility,
    abilityCooldown
}
public enum AbilityCheckType
{
    Melee,
    Ranged,
    Dash,
}

public class IfElseNode : CompositeNode
{
    public CheckType checkType;
    public float distanceCheck;
    public Optional<float> groupDistance;
    public Optional<float> groupCheckDistance;
    int first = 0; int second = 1;
    public AbilityCheckType abilityCheck;
    public Optional<oneTimeCheck> oneTime;
    public Optional<int> cooldownCheckIndex;
    public Optional<Ability> isAbility;
    public enum oneTimeCheck
    {
        Null,
        Doing,
        Completed,
    };

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    bool distCheck()
    {
        float distance = Vector3.Distance(agent.transform.position, blackboard.targetPosition);
        return checkType == CheckType.DistanceLessThan ? distance < distanceCheck : distance > distanceCheck;
    }

    bool retaliateCheck()
    {
        if (!agent.retaliate)
            return false;

        if (Time.time - agent.retaliateTimer > agent.retaliateCooldown)
        {
            agent.retaliate = false;
            return false;
        }
        else return true;

    }

    bool stunnedCheck()
    {
        if (!agent.isStunned)
            return false;

        if(Time.time - agent.stunnedTimer > agent.stunDuration)
        {
            agent.isStunned = false;
            return false;
        }
        return true;
    }

    int healthCheck()
    {
        for (int i = 0; i < agent.healthState.Value.Length; i++)
        {
            if (i == agent.healthState.Value.Length - 1) return i;

            if (agent.health.health > agent.healthState.Value[i] && agent.health.health <= agent.healthState.Value[i + 1])
            {
                if (oneTime.Value == oneTimeCheck.Doing)
                {
                    Debug.Log("OneTime");
                    oneTime.Value = oneTimeCheck.Completed;
                }
                return i + 1;
            }
        }
        return 0;
    }

    float groupDistanceCheck()
    {
        float average = 0;
        int count = 0;
        for (int i = 0; i < manager.enemyList.Count; i++)
        {
            if (manager.enemyList[i] != agent && !manager.enemyList[i].Seperating())
            {
                if (manager.enemyList[i] != null)
                {
                    float dist = Vector3.Distance(agent.transform.position, manager.enemyList[i].transform.position);
                    if (dist < groupCheckDistance.Value)
                    {
                        count++;
                        average += dist;
                    }
                }
            }
        }
        if (count == 0)
            return 1000000;

        average = average / manager.enemyList.Count;
        return average;
    }

    float groupFacingCheck()
    { 
        int count = 0;
        for(int i  = 0; i < manager.enemyList.Count; i++)
        {
            if(Vector3.Dot(player.transform.forward, (manager.enemyList[i].transform.position - 
                player.transform.position).normalized) > 0)
            {
                count++;
            }
        }
        return count;
    }

    bool delayMoveCheck()
    {
        if (agent.delayMove)
            return false;

        if(Time.time - agent.delayMoveTimer > agent.delayMoveRange)
        {
            agent.delayMove = false;
            return true;
        }

        return true;
    }

    bool sightLineCheck()
    {
        Vector3 offset = new Vector3(player.transform.position.x - agent.transform.position.x,
            (player.transform.position.y + 1) - (agent.transform.position.y), player.transform.position.z - agent.transform.position.z);
        RaycastHit hit;
        if (Physics.Raycast(agent.transform.position, offset, out hit))
        {
            if (hit.transform == player.transform)
            {
                return true;
            }
            else return false;
        }

        return false;
    }

    bool abilityTypeCheck(AbilityCheckType check)
    {
        if (!caster.curAbility)
            return false;

        Ability abil = agent.caster.curAbility;
        switch (check)
        {
            case AbilityCheckType.Melee:
                return abil.GetType() == typeof(MeleeAttackAbility);

            case AbilityCheckType.Ranged:
                return abil.GetType() == typeof(RangedAbility);

            case AbilityCheckType.Dash:
                return abil.GetType() == typeof(DashAbility) || abil.GetType() == typeof(DashApplysEffect);
            default:
                return false;
        }
    }

    bool abilityCooldownCheck()
    {
        AbilityCaster aCaster = agent.caster.caster;
        MeleeAttackAbility melee = aCaster.abilities[cooldownCheckIndex.Value] as MeleeAttackAbility;
        DashAbility dash = aCaster.abilities[cooldownCheckIndex.Value] as DashAbility;
        DashApplysEffect dasheffect = aCaster.abilities[cooldownCheckIndex.Value] as DashApplysEffect;
        if (melee || dash || dasheffect)
        {
            float cooldown = dash != null ? dash.GetCoolDownPercent() : melee.GetCoolDownPercent();
            if (cooldown < 0.9f)
            {
                return false;
            }
        }

        return true;
    }

    bool isAbilityCheck()
    {
        EnemyAbilityCaster aCaster = agent.caster;
        return aCaster.curAbility == isAbility.Value;
    }

    protected override State OnUpdate()
    {
        if (oneTime.Value != oneTimeCheck.Null && oneTime.Value == oneTimeCheck.Completed) 
        { 
            ChildUpdate(1);
        }


        switch (checkType)
        {
            #region distanceChecks
            //distance checks
            case CheckType.DistanceLessThan:
                if (distCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
            case CheckType.DistanceGreaterThan:
                if (distCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
            #endregion

            //health check
            case CheckType.Health:
                ChildUpdate(healthCheck());
                break;

            #region behaviourChecks
            //waiting to take retaliate action
            case CheckType.retaliate:
                if (retaliateCheck())
                    ChildUpdate(first);
                else ChildUpdate(second);
                break;

            case CheckType.isStunned:
                if (stunnedCheck())
                    ChildUpdate(first);
                else ChildUpdate(second);
                break;

            case CheckType.flanking:
                if (agent.flanking)
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            //currently waiting to move
            case CheckType.delayMove:
                if (delayMoveCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
            #endregion

            #region groupChecks
            //too many enemies in front of player
            case CheckType.groupFacing:
                if (groupFacingCheck() > manager.moveCount)
                    ChildUpdate(first);
                else 
                    ChildUpdate(second);
                break;
            //cluttered together
            case CheckType.groupDistance:
                if (groupDistanceCheck() < groupDistance.Value)
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
            #endregion

            //can the enemy see the player
            case CheckType.sightLine:
                if (sightLineCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            #region abilityChecks
            case CheckType.abilityType:
                if (abilityTypeCheck(abilityCheck))
                {
                    ChildUpdate(first);
                }
                else
                {
                    ChildUpdate(second);
                }

                break;
            case CheckType.abilityCooldown:
                if (abilityCooldownCheck())
                    ChildUpdate(first);
                else ChildUpdate(second);
                break;

            case CheckType.isAbility:
                if (isAbilityCheck())
                    ChildUpdate(first);
                else ChildUpdate(second);
                break;
                #endregion
        }
        return State.Running;
    }

    public State ChildUpdate(int index)
    {
        children[index].Update();
        return State.Running;
    }
}
