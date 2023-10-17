using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CheckType
{
    DistanceLessThan,
    DistanceGreaterThan,
    Health,
    isMoving,
    isDoingAction,
    isInDanger,
    retaliate,
    delayMove,
    groupDistance,
    groupFacing,
    flanking,
    sightLine,
    ability
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
    public float groupDistance;
    public float groupCheckDistance;
    int first = 0; int second = 1; //used for readability
    public AbilityCheckType abilityCheck;
    public oneTimeCheck oneTime;
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

    bool isMovingReset()
    {
        if (agent.isMoving && Time.time - agent.movementTimer > agent.movementCooldown)
        {
            agent.isMoving = false;
            return true;
        }
        return false;
    }

    bool performingActionReset()
    {
        if (agent.performingAction)
        {
            if (Time.time - agent.actionTimer > agent.actionCooldown)
            {
                agent.performingAction = false;
                return true;
            }
            return false;
        }
        else
            return true;
    }

    bool retaliateCheck()
    {
        if (!agent.retaliate)
            return false;

        if (Time.time - agent.retreatingTimer > agent.retaliateCooldown)
        {
            agent.retaliate = false;
            return false;
        }
        else return true;

    }

    int healthCheck()
    {
        for (int i = 0; i < agent.healthState.Value.Length; i++)
        {
            if (i == agent.healthState.Value.Length - 1) return i;

            if (agent.health.health > agent.healthState.Value[i] && agent.health.health <= agent.healthState.Value[i + 1])
            {
                if (oneTime == oneTimeCheck.Doing)
                    oneTime = oneTimeCheck.Completed;

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
                float dist = Vector3.Distance(agent.transform.position, manager.enemyList[i].transform.position);
                if (dist < groupCheckDistance)
                {
                    count++;
                    average += dist;
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
        //return false;
    }
    
    protected override State OnUpdate()
    {
        if (oneTime != oneTimeCheck.Null && oneTime == oneTimeCheck.Completed)
            ChildUpdate(0);

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
            //curently moving check
            case CheckType.isMoving:
                if (!isMovingReset())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            //currently doing action
            case CheckType.isDoingAction:
                if (!performingActionReset())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            //currently in danger check
            case CheckType.isInDanger:
                if (agent.isInDanger)
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            //waiting to take retaliate action
            case CheckType.retaliate:
                if (retaliateCheck())
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
                if (groupDistanceCheck() < groupDistance)
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

            case CheckType.ability:
                if (abilityTypeCheck(abilityCheck))
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
        }
        return State.Running;
    }

    public State ChildUpdate(int index)
    {
        children[index].Update();
        return State.Running;
    }
}
