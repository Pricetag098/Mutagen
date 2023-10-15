using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

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

    [Header("DropStats")]
    public Optional<Ability> setDrop;
    [Range(0,100)] public float dropChance;
    [HideInInspector] public Optional<Ability> droppedAbility;

    //behaviour bools
    [HideInInspector] public bool isMoving;
    [HideInInspector] public bool performingAction;
    [HideInInspector] public bool isInDanger;
    [HideInInspector] public bool delayMove;
    [HideInInspector] public bool flanking;
    [HideInInspector] public bool retaliate;

    [Header("Declutter Stats")]
    public float retreatingTimer = 2;
    [HideInInspector] public bool isSeperating;
    float lastSeperate;

    [Header("Stats")]
    public float movementMultiplier = 1;
    [Tooltip("In order of lowest to highest")]
    public Optional<int[]> healthState;
    public float flankDistance = 5;
    float defaultSpeed;

    [Header("Timers")]
    public float actionCooldown;
    public float movementSpeed;
    public float movementCooldown;
    public float retaliateCooldown;
    [Range(0f,10f)]
    public float delayMoveRange;
    [HideInInspector] public float defaultMovementSpeed;
    [HideInInspector] public float actionTimer;
    [HideInInspector] public float movementTimer;
    [HideInInspector] public float delayMoveTimer;
    [HideInInspector] public float retaliateTimer;

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
        manager = FindObjectOfType<EnemyManager>().GetComponent<EnemyManager>();
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
        this.enabled = false;
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
        retaliate = true;
        retaliateTimer = Time.time;
    }

    //randomlydrop ability on death, can set dedicated drop.
    void OnDie(DamageData data)
    {
        if (setDrop.Enabled)
        {
            droppedAbility.Enabled = true;
            droppedAbility.Value = setDrop.Value;
            //drop = Instantiate(setDrop.Value.pickupPrefab.Value);
            //Vector3 offset = transform.position;
            //offset.y += 1;
            //drop.transform.position = offset;
        }
        else
        {
            DropAbility();
        }

        manager.enemyList.Remove(this);
    }

    void DropAbility()
    {
        float dropping = Random.Range(0, 100);
        if(dropping > dropChance)
        {
            return;
        }

        int randDrop = Random.Range(0, caster.caster.abilities.Count() - 1);
        if (caster.curLoadout.abilities[randDrop].pickupPrefab.Enabled)
        {

            //drop = Instantiate(caster.caster.abilities[randDrop].pickupPrefab.Value);
        }
        droppedAbility.Enabled = true;
        droppedAbility.Value = caster.caster.abilities[randDrop];
        //offsets position to avoid spawning in ground
        //Vector3 offset = transform.position;
        //offset.y += 1;
        //drop.transform.position = offset;
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
