using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationSpeed : SetterNode
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
        agent.anim.Value.SetFloat("AnimationSpeed", speed);
        return child.Update();
    }
}
