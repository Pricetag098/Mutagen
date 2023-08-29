using System.Collections;
using System.Collections.Generic;

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
    [HideInInspector] public BehaviourTreeRunner behaviourTree;
    public Animator anim;
    public EnemyManager manager;

    //behaviour bools
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool performingAction;
    [HideInInspector] public bool isInDanger;
    [HideInInspector] public bool delayMove;
    [HideInInspector] public bool flanking;

    [Header("Stats")]
    public float movementMultiplier = 1;
    public int[] healthState;
    public float circlingDistance = 5;
    float defaultSpeed;

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
        behaviourTree = GetComponent<BehaviourTreeRunner>();
        //anim = caster.GetComponent<Animator>();

        defaultSpeed = movementSpeed;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    anim.Play("Headbutt");
        //}
    }

    public void ChangeMovementSpeed(float speed)
    {
        movementSpeed = speed;
        agent.speed = movementSpeed;
    }

    public void ChangeMovementMultiplier(float multi)
    {
        movementMultiplier = multi;
        ChangeMovementSpeed(movementSpeed * movementMultiplier);
    }

    public void Flank()
    {
        Vector3 playerFlank = player.transform.position + (-player.transform.forward * circlingDistance);
        agent.SetDestination(playerFlank);
    }

    public void KnockBack(Vector3 knockbackDirection) //set the ai's nav object position to give "skitter" effect
    {
        transform.position +=  knockbackDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.transform.gameObject.layer == hazardLM)
        //{
        //    dangerObject = other.transform.gameObject;
        //    isInDanger = true;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if(other.transform == dangerObject)
        //{
        //    dangerObject = null;
        //    isInDanger = false;
        //}
    }
}
