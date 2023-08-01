using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    Vector3 moveToPositionDefault;
    public GameObject moveToObject;
    GameObject moveToObjectDefault;
    public Vector3 targetPosition;
    //public AbilityCaster abilityCaster;
    //public EnemyAbilityCaster enemyAbilityCaster;
    
    public void Reset()
    {
        moveToPosition = moveToPositionDefault;
        moveToObject = moveToObjectDefault;
    }
}