using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/States/RamMovement"))]
public class RamMovement : State
{
    [SerializeField] float agentSpeed;
    public float speed = 1;
    public float distanceFromCentre = 30;

    int curPointIndex = 0;

    public override void OnEnter()
    {
        manager.nav.speed = agentSpeed;
        manager.movementPoint.speed = speed;
        manager.movementPoint.movementType = MovementType.ramming;
        manager.movementPoint.distanceFromCentre = distanceFromCentre;
        manager.movementTarget = manager.movementPoint.transform;
        manager.nav.angularSpeed = 1000;

        curPointIndex = 0;
        manager.nav.SetDestination(manager.movementPoint.rammingPoints[curPointIndex].transform.position);
    }

    public override void OnExit()
    {
        curPointIndex = 0;
    }

    public override void Tick()
    {

        if(Vector3.Distance(manager.agent.transform.position, manager.movementPoint.rammingPoints[curPointIndex].transform.position) < 7)
        {
            curPointIndex++;
            if (curPointIndex == manager.movementPoint.rammingPoints.Length)
                curPointIndex = 0;
            manager.nav.SetDestination(manager.movementPoint.rammingPoints[curPointIndex].transform.position);
        }

    }
}
