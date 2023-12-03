using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    public PlayerAbilityCaster player;
    [HideInInspector] public NavMeshAgent agent;
    public Health health;
    public Optional<Animator> anim;
    public EnemyManager manager;
    [HideInInspector] public EnemyAbilityCaster caster;
    public Renderer[] renderer;

    [Header("Optional References")]
    public Optional<BehaviourTreeRunner> behaviourTree;
    public Optional<Material> invisMat;
    public Optional<PipeColourChanger> pipeColourChanger;
    [HideInInspector] public EventManager eventManager;
    Material defaultMat;
    public Optional<GameObject[]> randoms;
    [HideInInspector] public delegate void OnActivate();
    public OnActivate onActivate;

    //behaviour bools
    [HideInInspector] public bool delayMove;
    [HideInInspector] public bool flanking;
    [HideInInspector] public bool retaliate;
    [HideInInspector] public bool isStunned;
    [HideInInspector] public bool startedFiring;
    [HideInInspector] public bool onCooldown;
    bool hitEffect;

    [Header("Declutter Stats")]
    public float retreatingTimer = 2;
    [HideInInspector] public bool isSeperating;
    float lastSeperate;

    [Header("Stats")]
    public float movementSpeed;
    public float movementMultiplier = 1;
    public float flankDistance = 5;
    [Range(0.1f, 50f)]
    public float knockbackResist;
    float defaultSpeed;
    [Tooltip("In order of lowest to highest")]
    public Optional<int[]> healthState;
    int hitCount;

    [Header("Timers")]
    public float retaliateCooldown;
    public float stunDuration;
    [Range(0f, 1f)]
    public float onHitFlashTime;
    [Range(0f,10f)]
    public float delayMoveRange;
    //move these to blackboard
    [HideInInspector] public float defaultMovementSpeed;
    [HideInInspector] public float delayMoveTimer;
    [HideInInspector] public float retaliateTimer;
    [HideInInspector] public float stunnedTimer;
    [HideInInspector] public float firingTimer;
    [HideInInspector] public float cooldownTimer;
    float hitFlashTimer;
    bool telegraph;
    float telegraphTimer;

    #region startupfunctions
    void Awake()
    {
        //referencing components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
        behaviourTree.Value = GetComponent<BehaviourTreeRunner>();
        eventManager = GetComponent<EventManager>();
        defaultMat = transform.parent.gameObject.GetComponentInChildren<Renderer>().material;
        player = FindObjectOfType<PlayerAbilityCaster>();
    }

    private void Start()
    {
        health.OnHit += OnHit;
        health.OnDeath += OnDie;
        defaultSpeed = movementSpeed;
    }

    //used for idle animations and starting to attack the player
    public void Activate()
    {
        if(behaviourTree.Enabled)
        behaviourTree.Value.enabled = true;
        if(anim.Enabled)
        anim.Value.SetTrigger("Detected");

        if(onActivate != null)
            onActivate();

    }

    public void Deactivate()
    {
        if (behaviourTree.Enabled)
            behaviourTree.Value.enabled = false;

        //this.enabled = false;
    }

    //randomizes parts
    public void Randomize()
    {
        if (!randoms.Enabled)
            return;

        int activeCount = Random.Range(1, 3);
        for (int i = 0; i < activeCount; i++)
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
        behaviourTree.Value.tree = newTree.Clone();
    }
    #endregion

    private void FixedUpdate()
    {
        if(anim.Enabled)
        anim.Value.SetFloat("Speed", agent.speed);

        ////changes pipecolour when about to attack
        //if (telegraph)
        //{
        //    if (Time.time - telegraphTimer > 0.1f)
        //    {
        //        telegraph = false;
        //        pipeColourChanger.Value.Default();
        //    }
        //}

        //on hit effect for player feedback
        if (!hitEffect)
            return;

        if(Time.time - hitFlashTimer > onHitFlashTime)
        {
            for (int i = 0; i < renderer.Count(); i++)
                renderer[i].material.SetFloat("_RimLight", 0);
            hitEffect = false;
        }

    }

    //public void AttackEffect()
    //{
    //    if (!pipeColourChanger.Enabled)
    //        return;
    //    telegraphTimer = Time.time;
    //    pipeColourChanger.Value.Change(pipeColourChanger.Value.materials.Length - 1);
    //}

    //removes enemy from manager list
    void OnDie(DamageData data)
    {
        manager.Remove(this);
        manager.enemyList.Remove(this);
        enabled = false;
    }

    #region movement
    public void Flank()
    {
        Vector3 playerFlank = player.transform.position + (-player.transform.forward * flankDistance);
        agent.SetDestination(playerFlank);
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
    #endregion

    #region onHitEffects
    void OnHit(DamageData data)
    {
        //if not activated, activate all in managers list
        if (!manager.activated)
            manager.Activate();

        //hit stun armour
        hitCount++;

        //activate flash effect, will change depending on hit armour count
        for (int i = 0; i < renderer.Count(); i++)
        {
            if (hitCount >= 3)
            {
                renderer[i].material.SetFloat("_RimLight", 1);
                renderer[i].material.SetColor("_RimLightColor", new Color(0, 0, 1, 1));
                hitCount = 0;
            }
            else
            {
                renderer[i].material.SetFloat("_RimLight", 1);
                renderer[i].material.SetColor("_RimLightColor", new Color(1, 1, 0, 1));
            }
        }

        hitEffect = true;
        hitFlashTimer = Time.time;

        //retaliate
        retaliate = true;
        retaliateTimer = Time.time;
    }

    public void SetStunned(float stunTime)
    {
        if (hitCount >= 3)
            return;
        stunDuration = stunTime;
        isStunned = true;
        stunnedTimer = Time.time;
    }

    public void KnockBack(Vector3 knockbackDirection) //set the ai's nav object position to give "skitter" effect
    {
        if(hitCount >= 3)
            knockbackDirection = Vector3.zero;
        else
        knockbackDirection /= knockbackResist;
        transform.position +=  knockbackDirection;
    }
    #endregion
}
