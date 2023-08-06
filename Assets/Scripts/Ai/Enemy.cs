using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Health health;
    [HideInInspector] public EnemyAbilityCaster caster;
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool performingAction;
    [HideInInspector] public bool isInDanger;
    [HideInInspector] public GameObject dangerObject; //used for dodging, will look into cleaner way of doing
    public PlayerAbilityCaster player;
    [Header("Stats")]
    public float actionCooldown;
    public float movementSpeed;
    public float movementCooldown;
    public float movementMultiplier = 1;
    public int[] healthState;
    [HideInInspector] public float defaultMovementSpeed;
    [HideInInspector] public float actionTimer;
    [HideInInspector] public float movementTimer;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
    }

    public void ChangeMovementSpeed(float speed)
    {
        movementSpeed = speed;
        agent.speed = movementSpeed;
    }

    public void ChangeMovementMultiplier(float multi)
    {
        movementMultiplier = multi;
        ChangeMovementSpeed(movementSpeed * multi);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 awayDir = new Vector3(transform.position.x - player.transform.position.x, 
            transform.position.y, transform.position.z - player.transform.position.z) * 1.1f;
        Gizmos.DrawLine(transform.position, awayDir);
    }
}
