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
    isInDanger
}

public class IfElseNode : CompositeNode
{
    public CheckType checkType;
    public float distanceCheck;
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
        bool test = checkType == CheckType.DistanceLessThan ? distance < distanceCheck : distance > distanceCheck;
        return test;
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
                if (agent.isMoving)
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;

            //currently doing action
            case CheckType.isDoingAction:
                if (agent.performingAction)
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
        }
        return State.Running;
    }

    public State ChildUpdate(int index)
    {
        children[index].Update();
        return State.Running;
    }
}
