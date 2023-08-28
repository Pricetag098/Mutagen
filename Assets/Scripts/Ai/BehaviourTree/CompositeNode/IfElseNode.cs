using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckType
{
    DistanceLessThan,
    DistanceGreaterThan,
    Health,
    isMoving,
    isDoingAction,
    isInDanger,
    delayMove,
    groupDistance,
    groupFacing,
    flanking,
    sightLine
}

public class IfElseNode : CompositeNode
{
    public CheckType checkType;
    public float distanceCheck;
    public float groupDistance;
    int first = 0; int second = 1; //used for readability

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

    int healthCheck()
    {
        int heal = agent.healthState[0];
        for (int i = 0; i < agent.healthState.Length; i++)
        {
            if (agent.health.health > agent.healthState[i] && agent.health.health < agent.healthState[i + 1])
            {
                return i;
            }
        }
        return agent.healthState.Length - 1;
    }

    float groupDistanceCheck()
    {
        float average = 0;
        int count = 0;
        for (int i = 0; i < manager.enemyList.Count; i++)
        {
            if (manager.enemyList[i] != agent)
            {
                float dist = Vector3.Distance(agent.transform.position, manager.enemyList[i].transform.position);
                if (dist < groupDistance)
                {
                    count++;
                    average += dist;
                }
            }
        }
        if (count == 0)
            return 1000000;

        float returns = average / manager.enemyList.Count;
        //Debug.Log(average / manager.enemyList.Length);
        //return count > 0 ? (average / count) : 0;
        return average / manager.enemyList.Count;//count;
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
    
    protected override State OnUpdate()
    {
        switch (checkType)
        {
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

            //health check
            case CheckType.Health:
                ChildUpdate(healthCheck());
                break;

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


            case CheckType.sightLine:
                if (sightLineCheck())
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
