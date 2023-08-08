using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToList : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (!manager.inFront.Contains(agent))
        manager.inFront.Add(agent);

        return State.Success;
    }
}
