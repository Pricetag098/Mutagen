using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceAttackTimer : SetterNode
{
    public float reduceMultiplier;


    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        return child.Update();
    }
}
