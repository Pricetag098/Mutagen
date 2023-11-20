using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/States/RotationMovement"))]
public class RotationMovement : State
{
    [SerializeField] float agentSpeed;
    public float speed = 1;
    public float distanceFromCentre = 30;

    public override void OnEnter()
    {
        manager.nav.speed = agentSpeed;
        manager.movementPoint.speed = speed;
        manager.movementPoint.movementType = MovementType.rotating;
        manager.movementPoint.distanceFromCentre = distanceFromCentre;
        manager.movementTarget = manager.movementPoint.rotating.transform;
    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {
        manager.nav.SetDestination(manager.movementTarget.position);
    }

}
