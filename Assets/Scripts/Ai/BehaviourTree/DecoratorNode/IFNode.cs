using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFNode : DecoratorNode
{
    public CheckType checkType;


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
                else  return State.Failure;

            case CheckType.isMoving:
                if (!isMovingReset())
                {
                    child.Update();
                    return State.Running;
                }
                else return State.Failure;
        }
        return State.Running;
    }
}
