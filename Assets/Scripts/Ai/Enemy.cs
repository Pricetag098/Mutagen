using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public PlayerAbilityCaster player;
    [HideInInspector] public NavMeshAgent agent;
    public Health health;
    public Animator anim;
    public EnemyManager manager;
    [HideInInspector] public EnemyAbilityCaster caster;
    public BehaviourTreeRunner behaviourTree;

    [Header("Optional References")]
    public Optional<Material> invisMat;
    public Optional<PipeColourChanger> pipeColourChanger;
    [HideInInspector] public EventManager eventManager;
    Material defaultMat;
    public Optional<GameObject[]> randoms;

    //behaviour bools
    [HideInInspector] public bool delayMove;
    [HideInInspector] public bool flanking;
    [HideInInspector] public bool retaliate;
    [HideInInspector] public bool isStunned;

    [Header("Declutter Stats")]
    public float retreatingTimer = 2;
    [HideInInspector] public bool isSeperating;
    float lastSeperate;

    [Header("Stats")]
    public float movementSpeed;
    public float movementMultiplier = 1;
    [Tooltip("In order of lowest to highest")]
    public Optional<int[]> healthState;
    public float flankDistance = 5;
    float defaultSpeed;

    [Header("Timers")]
    public float retaliateCooldown;
    public float stunDuration;
    [Range(0f,10f)]
    public float delayMoveRange;
    [HideInInspector] public float defaultMovementSpeed;
    [HideInInspector] public float delayMoveTimer;
    [HideInInspector] public float retaliateTimer;
    [HideInInspector] public float stunnedTimer;

    #region startupfunctions
    void Awake()
    {
        //referencing components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
        behaviourTree = GetComponent<BehaviourTreeRunner>();
        eventManager = GetComponent<EventManager>();
        defaultMat = transform.parent.gameObject.GetComponentInChildren<Renderer>().material;
        player = FindObjectOfType<PlayerAbilityCaster>().GetComponent<PlayerAbilityCaster>();
        //manager = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
    }

    private void Start()
    {
        health.OnHit += OnHit;
        health.OnDeath += OnDie;
        defaultSpeed = movementSpeed;
    }

    private void FixedUpdate()
    {
        anim.SetFloat("Speed", agent.speed);
    }

    //used for idle animations and starting to attack the player
    public void Activate()
    {
        behaviourTree.enabled = true;
        anim.SetTrigger("Detected");
    }

    public void Deactivate()
    {
        behaviourTree.enabled = false;
        //this.enabled = false;
    }

    //randomizes parts
    public void Randomize()
    {
        if (!randoms.Enabled)
            return;

        int activeCount = Random.Range(1, 3);
        for(int i = 0; i < activeCount; i++)
        {
            int active = Random.Range(0, 3);
            if (!randoms.Value[active].activeInHierarchy)
            {
                if (active != 2)
                    active++;
                else
                    active = 0;
            }
                randoms.Value[active].active = false;
        }

        if (randoms.Value.Length < 3)
            return;

        activeCount = Random.Range(0, 3);
        for (int i = 0; i < activeCount; i++)
        {
            int active = Random.Range(3, 6);
            if (!randoms.Value[active].activeInHierarchy)
            {
                if (active != 5)
                    active++;
                else
                    active = 0;
            }
                randoms.Value[active].active = false;
        }
    }

    public void BindTree(BehaviourTree newTree)
    {
        behaviourTree.tree = newTree.Clone();
    }
    #endregion

    void OnHit(DamageData data)
    {
        if (!manager.activated)
            manager.Activate();
        retaliate = true;
        retaliateTimer = Time.time;
    }

    public void SetStunned(float stunTime)
    {
        stunDuration = stunTime;
        isStunned = true;
        stunnedTimer = Time.time;
    }

    //removes ememy from manager list
    void OnDie(DamageData data)
    {
        manager.Remove(this);
        manager.enemyList.Remove(this);
        //enabled = false;
    }

    //speed functions
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

    //movement functions
    public void Flank()
    {
        Vector3 playerFlank = player.transform.position + (-player.transform.forward * flankDistance);
        agent.SetDestination(playerFlank);
    }

    public void KnockBack(Vector3 knockbackDirection) //set the ai's nav object position to give "skitter" effect
    {
        transform.position +=  knockbackDirection;
    }

    public bool Seperating()
    {
        if (isSeperating)
        {
            if (Time.time - lastSeperate > retreatingTimer)
            {
                isSeperating = false;
                return false;
            }
            return true;
        }
        lastSeperate = Time.time;
        isSeperating = true;
        return true;
    }
}
