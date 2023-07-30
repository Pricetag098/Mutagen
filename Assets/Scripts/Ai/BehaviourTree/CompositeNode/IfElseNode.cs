using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfElseNode : CompositeNode
{
    public CheckType checkType;
    public float distanceCheck;

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
            case CheckType.LessThan:
                if (distCheck())
                {
                    children[0].Update();
                    return State.Running;
                }
                else
                {
                    children[1].Update();
                    return State.Running;
                }
            case CheckType.GreaterThan:
                if (distCheck())
                {
                    children[0].Update();
                    return State.Running;
                }
                else
                {
                    children[1].Update();
                    return State.Running;
                }
                //case other checks e.g. indanger
        }
        return State.Running;
    }
}
