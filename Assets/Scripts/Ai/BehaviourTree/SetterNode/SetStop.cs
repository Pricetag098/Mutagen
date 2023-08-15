using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStop : SetterNode
{
    float speed;
    protected override void OnStart()
    {
        speed = agent.agent.speed;
        agent.agent.speed = 0;
    }

    protected override void OnStop()
    {

        agent.agent.speed = speed;
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }
}
