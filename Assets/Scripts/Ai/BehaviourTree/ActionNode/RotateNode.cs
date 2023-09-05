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
        Vector3 pos = (blackboard.rotateTowardsObject.transform.position - agent.transform.position);
        float angleToPosition = Vector3.Angle(agent.transform.forward, pos);
        float dot = Vector3.Dot(-agent.transform.right,
            (blackboard.rotateTowardsObject.transform.position - agent.transform.position).normalized);

        if (angleToPosition < angle)
        {
            return State.Success;
        }

        //agent.agent.SetDestination(player.transform.position);

        //if (dot > 0)
        //    agent.transform.Rotate((blackboard.rotateTowardsObject.transform.position - agent.transform.position).normalized
        //        * rotateSpeed * Time.fixedDeltaTime);
        //else
        //    agent.transform.Rotate(-(blackboard.rotateTowardsObject.transform.position - agent.transform.position).normalized
        //        * rotateSpeed * Time.fixedDeltaTime);

        agent.transform.LookAt(blackboard.rotateTowardsObject.transform.position);



        return State.Running;
    }
}
