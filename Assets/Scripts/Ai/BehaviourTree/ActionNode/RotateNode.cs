using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNode : ActionNode
{
    public float rotateSpeed;
    public float angle;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        //agent.transform.LookAt(agent.player.transform.position);
        Vector3 pos = (agent.player.transform.position - agent.transform.position);
        float angleToPosition = Vector3.Angle(agent.transform.forward, pos);


        Debug.Log(angleToPosition);
        if (angleToPosition < angle)
        {
            return State.Success;
        }
        agent.transform.Rotate((agent.player.transform.position - agent.transform.position)
            * rotateSpeed * Time.fixedDeltaTime);

        return State.Running;
    }
}
