using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatUntilFail : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Success;
            case State.Success:
                return State.Running;
            default:
                return State.Success;
        }
    }
}
