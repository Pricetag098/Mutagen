using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    Vector3 moveToPositionDefault;
    public Vector3 moveAwayObject;
    public GameObject moveToObject;
    GameObject moveToObjectDefault;
    public GameObject rotateTowardsObject;
    public Vector3 targetPosition;
    public Ability[] abilities;
    [HideInInspector] float delayMoveTimer;


    
    public void Reset()
    {
        moveToPosition = moveToPositionDefault;
        moveToObject = moveToObjectDefault;
    }
}