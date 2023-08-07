using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGroupDirection : SetterNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    Vector3 groupDirection()
    {
        Vector3 dir = Vector3.zero;

        for(int i = 0; i < manager.enemyList.Length; i++)
        {
            if(manager.enemyList[i] != agent)
            {
                //do distance check later

                dir += new Vector3(agent.transform.position.x - manager.enemyList[i].transform.position.x, 0,
                    agent.transform.position.z - manager.enemyList[i].transform.position.z).normalized;
            }
        }

        return (agent.transform.position + (dir.normalized * agent.movementSpeed));
    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = groupDirection();

        return child.Update();
    }
}
