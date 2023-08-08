using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckType
{
    LessThan,
    GreaterThan,
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
        bool test = checkType == CheckType.LessThan ? distance < distanceCheck : distance > distanceCheck;
        return test;
        //else if
    }

    protected override State OnUpdate()
    {

        switch (checkType)
        {
            //distance checks
            case CheckType.LessThan:
                if (distCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);
                break;
            case CheckType.GreaterThan:
                if (distCheck())
                    ChildUpdate(first);
                else
                    ChildUpdate(second);

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
