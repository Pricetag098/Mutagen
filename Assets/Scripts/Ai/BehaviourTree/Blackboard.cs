using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Blackboard
{
    [HideInInspector] public Vector3 moveToPosition;
    [HideInInspector] public GameObject rotateTowardsObject;
    [HideInInspector] public Vector3 targetPosition;
    public Ability[] abilities;
    public EnemyManager manager;
    float delayMoveTimer;
    [HideInInspector] public Vector3 awayPosition;

}