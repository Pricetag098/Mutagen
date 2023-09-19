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
    [HideInInspector] public bool isRetreating;
    public bool retreating() 
    {
        if (isRetreating)
        {
            if(Time.time - lastRetreat > retreatingTimer)
            {
                isRetreating = false;
                return false;
            }
            return true;
        }
        lastRetreat = Time.time;
        isRetreating = true;
        return true;
    }
    public float retreatingTimer = 2;
    float lastRetreat;

    [Header("Stats")]
    public float movementMultiplier = 1;
    [Tooltip("In order of lowest to highest")]
    public Optional<int[]> healthState;
    public float circlingDistance = 5;
    float defaultSpeed;
    public bool setDrop; 
    public int setDropIndex;

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



    void Start()
    {
        //referencing components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
        behaviourTree = GetComponent<BehaviourTreeRunner>();
        eventManager = GetComponent<EventManager>();
        defaultMat = transform.parent.gameObject.GetComponentInChildren<Renderer>().material;

        health.OnHit += OnHit;
        health.OnDeath += OnDie; 
        defaultSpeed = movementSpeed;
    }

    public void BindTree(BehaviourTree newTree)
    {
        behaviourTree.tree = newTree.Clone();
    }

    void Update()
    {


    }

    void OnHit(DamageData data)
    {
        retaliate = true;
    }

    void OnDie(DamageData data)
    {
        manager.enemyList.Remove(this);


        int randDrop = Random.Range(0, caster.caster.abilities.Count() - 1);

        if (setDrop)
        {
            GameObject drop = Instantiate(caster.caster.abilities[setDropIndex].pickupPrefab.Value);
            Vector3 offset = transform.position;
            offset.y += 1;
            drop.transform.position = offset;
        }


        if (caster.curLoadout.abilities[randDrop].pickupPrefab.Enabled)
        {
            GameObject drop = Instantiate(caster.caster.abilities[randDrop].pickupPrefab.Value);
            Vector3 offset = transform.position;
            offset.y += 1;
            drop.transform.position = offset;
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
