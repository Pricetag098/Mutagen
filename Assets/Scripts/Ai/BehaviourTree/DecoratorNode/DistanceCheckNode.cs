using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DistanceCheckNode : DecoratorNode
{
    public float distanceCheck;
    public GameObject checkingObject;
    [SerializeField] CheckType checkType;

    protected override void OnStart()
    {
        //blackboard.targetPosition = agent.player.gameObject.transform.position;
    }

    protected override void OnStop()
    {

    }

    bool check()
    {
        float distance = Vector3.Distance(agent.transform.position, blackboard.targetPosition);
        if (checkType == CheckType.LessThan)
        {
            if (distance < distanceCheck)
            {
                return true;
            }
        }
        else
        {
            if (distance > distanceCheck)
            {
                return true;
            }
        }
        return false;

    }

    protected override State OnUpdate()
    {
        if (check())
        {
            child.Update();
            return State.Running;
        }

        return State.Failure;


    }
}
