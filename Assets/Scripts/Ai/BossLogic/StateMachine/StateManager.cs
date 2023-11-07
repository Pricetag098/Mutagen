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
    public Transform castOrigin;
    public MovingPoint movementPoint;
    [HideInInspector] public Transform movementTarget;

    [Header("States")]
    public State[] states;
    State curState;
    public bool casting;

    //readability
    int rotating = 0; int chompAttack = 1;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        caster = GetComponent<EnemyAbilityCaster>();
        player = FindObjectOfType<PlayerMovement>();
        agent = FindObjectOfType<Enemy>();
        foreach(State state in states)
        {
            state.manager = this;
        }

        curState = states[0];
        curState.OnEnter();
    }

    private void Update()
    {
        //run current state
        curState.Tick();

        //if not already casting ability
        if (!casting)
        {
            //if player is in front, do bite attack
            RaycastHit hit;
            Vector3 cast = castOrigin.transform.position + transform.forward * 50;
            cast.y = transform.position.y + 10;
            if (Physics.SphereCast(castOrigin.position, 50f, cast, out hit, 101f, playerLayer))
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
            Debug.Log("Cast");
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
        Vector3 draw = castOrigin.transform.position + transform.forward * 50;
        draw.y = transform.position.y;
        Gizmos.DrawLine(transform.position, draw);
    }
}
