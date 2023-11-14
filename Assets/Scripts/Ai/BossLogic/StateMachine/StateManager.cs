using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public NavMeshAgent nav;
    [HideInInspector] public Enemy agent;
    [HideInInspector] public EnemyAbilityCaster caster;
    [SerializeField] LayerMask playerLayer;
    [HideInInspector] public PlayerMovement player;
    public Transform[] castOrigins;
    public MovingPoint movementPoint;
    [HideInInspector] public Transform movementTarget;

    [Header("States and State logic")]
    public State[] states;
    State curState;
    public bool casting;

    [Header("Stats")]
    public float actionCooldown;
    [HideInInspector] public float actionTimer;

    //readability
    int rotating = 0; int chompAttack = 1; int empAttack = 2;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        caster = GetComponent<EnemyAbilityCaster>();
        player = FindObjectOfType<PlayerMovement>();
        agent = FindObjectOfType<Enemy>();
        for(int i = 0; i < states.Length; i++)
        {
            states[i] = Instantiate(states[i]);
            states[i].manager = this;
        }

        curState = states[0];
        curState.OnEnter();
    }

    private void Update()
    {
        //run current state
        curState.Tick();

        if (casting)
            return;


        //if player is in front, do bite attack
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 10f, transform.forward, out hit, 15f, playerLayer))
        {
            if (curState != states[chompAttack])
            {
                Debug.Log("Hit");
                curState.OnExit();
                curState = states[chompAttack];
                curState.OnEnter();
            }
            return;
        }
        else
        {

        }

        //normal movement
        if (curState != states[rotating])
        {
            curState.OnExit();
            curState = states[rotating];
            curState.OnEnter();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 draw =  transform.position + (transform.forward) * 20;
        draw.y = transform.position.y;
        Gizmos.DrawLine(transform.position, draw);
    }
}
