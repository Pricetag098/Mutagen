using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStopMoving : SetterNode
{

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = agent.transform.position;

        return child.Update();
    }
}
