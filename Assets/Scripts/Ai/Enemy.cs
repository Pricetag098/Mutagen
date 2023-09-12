using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public PlayerAbilityCaster player;
    [HideInInspector] public NavMeshAgent agent;
    public Health health;
    public Animator anim;
    public EnemyManager manager;
    public Optional<Material> invisMat;
    [HideInInspector] public EnemyAbilityCaster caster;
    [HideInInspector] public GameObject dangerObject;
    [HideInInspector] public BehaviourTreeRunner behaviourTree;
    [HideInInspector] public EventManager eventManager;
    Material defaultMat;

    //behaviour bools
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool performingAction;
    [HideInInspector] public bool isInDanger;
    [HideInInspector] public bool delayMove;
    [HideInInspector] public bool flanking;
    [HideInInspector] public bool retaliate;

    [Header("Stats")]
    public float movementMultiplier = 1;
    public Optional<int[]> healthState;
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
        eventManager = GetComponent<EventManager>();
        defaultMat = transform.parent.gameObject.GetComponentInChildren<Renderer>().material;
        //anim = caster.GetComponent<Animator>();


        health.OnHit += OnHit;
        health.OnDeath += OnDie; 
        defaultSpeed = movementSpeed;
    }

    public void BindTree(BehaviourTree newTree)
    {
        behaviourTree.tree = newTree.Clone();
    }

    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            caster.caster.DisableCast(5);
        }
    }

    void OnHit(DamageData data)
    {
        retaliate = true;
    }

    void OnDie(DamageData data)
    {
        int randDrop = Random.Range(0, caster.caster.abilities.Count() - 1);

        if (caster.caster.abilities[randDrop].pickupPrefab.Enabled)
        {
            GameObject drop = Instantiate(caster.caster.abilities[randDrop].pickupPrefab.Value);
            drop.transform.position = transform.position; //may need to change y pos
        }
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
