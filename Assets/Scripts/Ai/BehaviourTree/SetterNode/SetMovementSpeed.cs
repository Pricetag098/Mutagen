using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMovementSpeed : SetterNode
{
    public float speed;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        agent.ChangeMovementSpeed(speed);

        return child.Update();
    }
}
