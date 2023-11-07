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
        manager.movementPoint.speed = speed;
        manager.movementPoint.distanceFromCentre = distanceFromCentre;
        manager.movementTarget = manager.movementPoint.transform;
    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {
        manager.nav.SetDestination(manager.movementTarget.position);
    }

}
