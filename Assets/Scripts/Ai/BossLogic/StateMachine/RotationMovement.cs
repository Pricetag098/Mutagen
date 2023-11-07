using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/States/MoveStates/Rotation"))]
public class RotationMovement : MoveState
{
    public float speed = 1;
    public float distanceFromCentre = 30;

    public override void OnEnter()
    {
        MovingPoint point = manager.movementPoint.GetComponent<MovingPoint>();
        point.speed = speed;
        point.distanceFromCentre = distanceFromCentre;
        manager.movementTarget = manager.movementPoint;
    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {
        manager.agent.SetDestination(manager.movementTarget.position);
    }

}
