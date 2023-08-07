using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public PlayerAbilityCaster player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Health health;
    [HideInInspector] public EnemyAbilityCaster caster;
    [HideInInspector] public GameObject dangerObject; //used for dodging, will look into cleaner way of doing
    public EnemyManager manager;

    //behaviour bools
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool performingAction;
    [HideInInspector] public bool isInDanger;
    [HideInInspector] public bool delayMove;

    [Header("Stats")]
    public float movementMultiplier = 1;
    public int[] healthState;
    public float knockbackForce;

    [Header("Timers")]
    public float actionCooldown;
    public float movementSpeed;
    public float movementCooldown;
    [Range(0f,10f)]
    public float delayMoveRange;
    [HideInInspector] public float defaultMovementSpeed;
    [HideInInspector] public float actionTimer;
    [HideInInspector] public float movementTimer;
    [HideInInspector] public float delayMoveTimer;


    private void Start()
    {
        //referencing components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
        //manager = transform.parent.GetComponent<EnemyManager>();


        if (delayMoveRange == 0)
            delayMoveRange = Random.Range(0.1f,5f);
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

    public void KnockBack(Vector3 origin) //change force to input if needed
    {
        Vector3 awayDir = new Vector3(transform.position.x - origin.x,
        transform.position.y, transform.position.z - origin.z).normalized;
        transform.position += awayDir * knockbackForce;
    }
}
