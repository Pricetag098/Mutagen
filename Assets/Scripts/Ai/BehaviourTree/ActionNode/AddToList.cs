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
        //EnemyManager stalker = manager as StalkerManager;

        //if (!stalker.inFront.Contains(agent))
        //    stalker.inFront.Add(agent);

        return State.Success;
    }
}
