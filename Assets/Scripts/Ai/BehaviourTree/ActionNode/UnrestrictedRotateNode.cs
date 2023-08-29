using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrestrictedRotateNode : ActionNode
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
        //Vector3 pos = (blackboard.rotateTowardsObject.transform.position - agent.transform.position);
        //float angleToPosition = Vector3.Angle(agent.transform.forward, pos);
        float dot = Vector3.Dot(-agent.transform.right,
            (blackboard.rotateTowardsObject.transform.position - agent.transform.position).normalized);


        //if (dot > 0)
        //    agent.transform.Rotate((blackboard.rotateTowardsObject.transform.position - agent.transform.position)
        //        * rotateSpeed * Time.fixedDeltaTime);
        //else
            agent.transform.Rotate(-(blackboard.rotateTowardsObject.transform.position - agent.transform.position)
                * rotateSpeed * Time.fixedDeltaTime);


        //if (angleToPosition < angle)
        //{
        return State.Success;
        //}

        //Debug.Log(dot);



        //return State.Running;
    }
}
