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
    [HideInInspector] public HitBox hb;
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

    [Header("RBTest")]
    public GameObject[] colliders;


    private void Start()
    {
        //referencing components
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();    
        caster = GetComponent<EnemyAbilityCaster>();
        behaviourTree = GetComponent<BehaviourTreeRunner>();
        eventManager = GetComponent<EventManager>();
        defaultMat = transform.parent.gameObject.GetComponentInChildren<Renderer>().material;
        hb = transform.parent.GetComponentInChildren<HitBox>();
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
            Debug.Log("Ragdoll");

            //disable other colliders

            hb.transform.GetComponent<Collider>().enabled = false;

            for(int i = 0; i < transform.parent.childCount; i++)
            {
                SecondOrderFollower follow;
                if(transform.parent.GetChild(i).TryGetComponent<SecondOrderFollower>(out follow))
                {
                    follow.enabled = false;
                }

                SecondOrderFacer facer;
                if (transform.parent.GetChild(i).TryGetComponent<SecondOrderFacer>(out facer))
                {
                    facer.enabled = false;
                }

                Collider col;
                if (transform.parent.GetChild(i).TryGetComponent<Collider>(out col))
                {
                    col.enabled = false;
                }
            }

            //enable new colliders and turn on kinematic
            for(int i = 0; i < colliders.Length; i++)
            {
                colliders[i].GetComponent<Collider>().enabled = true;
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                //rb.AddForce(rb.transform.position + -(transform.position - rb.transform.position), ForceMode.Impulse);
            }

            //disable components
            anim.enabled = false;
            agent.enabled = false;
            this.enabled = false;
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
