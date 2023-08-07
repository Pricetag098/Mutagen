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

    protected override State OnUpdate()
    {
        switch (checkType)
        {
            case CheckType.isDoingAction:
                if (!agent.performingAction)
                {
                    child.Update();
                    return State.Running;
                }
                else
                    return State.Failure;

        }
        return State.Running;
    }
}
