using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFNode : DecoratorNode
{
    public CheckType checkType;
    public float groupAverage;


    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

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

    bool isMovingReset()
    {
        if (agent.isMoving && Time.time - agent.movementTimer > agent.movementCooldown)
        {
            agent.isMoving = false;
            return true;
        }
        return false;
    }

    float groupDistanceCheck()
    {
        float average = 0;
        for (int i = 0; i < manager.enemyList.Count; i++)
        {
            average += Vector3.Distance(agent.transform.position, manager.enemyList[i].transform.position);
        }
        Debug.Log(average / manager.enemyList.Count);
        return average / manager.enemyList.Count;
    }

    bool flankingCheck()
    {
        if (agent.flanking)
        {
            if(Vector3.Distance(agent.transform.position, agent.agent.destination) < 0.2f)
            {
                agent.flanking = false;
                manager.moving.Remove(agent);
                
                return false;
            }
            return true;
        }

        return false;
    }

    protected override State OnUpdate()
    {
        switch (checkType)
        {
            case CheckType.isDoingAction:
                if (performingActionReset())
                {
                    child.Update();
                    return State.Running;
                }
                return State.Failure;

            case CheckType.isMoving:
                if (!isMovingReset())
                {
                    child.Update();
                    return State.Running;
                }
                return State.Failure;

            case CheckType.groupDistance:
                if (groupDistanceCheck() < groupAverage)
                {
                    child.Update();
                    return State.Running;
                }
                return State.Failure;

            case CheckType.flanking:
                if (!flankingCheck())
                {
                    child.Update();
                    return State.Running;
                }
                return State.Failure;
        }
        return State.Running;
    }
}
