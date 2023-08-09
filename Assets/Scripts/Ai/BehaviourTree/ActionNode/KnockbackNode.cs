using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackNode : ActionNode
{
    public float forceAmount;
    Vector3 awayDir;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        return State.Success;
    }
}
