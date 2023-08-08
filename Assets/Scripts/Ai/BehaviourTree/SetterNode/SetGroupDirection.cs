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

    Vector3 groupDirection() //change later
    {
        Vector3 dir = Vector3.zero;

        for(int i = 0; i < manager.enemyList.Length; i++)
        {
            if(manager.enemyList[i] != agent)
            {
                dir += new Vector3(agent.transform.position.x - manager.enemyList[i].transform.position.x, 0,
                    agent.transform.position.z - manager.enemyList[i].transform.position.z).normalized;
            }
        }

        return (agent.transform.position + (dir.normalized * agent.movementSpeed));
    }

    Vector3 groupFacing()
    {
        Vector3 playerFlank = agent.player.transform.position + (-agent.player.transform.forward * agent.circlingDistance);
        return playerFlank;
        //manager.enemyList[0].agent.SetDestination(playerFlank);

    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition = groupDirection();
        //blackboard.moveToPosition = groupFacing();
        return child.Update();
    }
}
