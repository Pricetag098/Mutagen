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

    //special actions that are occasional attacks
    [HideInInspector] public bool canCastSpecial;
    [HideInInspector] public float specialAttackTimer;
    [HideInInspector] public float specialAttackCooldown;

    //readability, phase 1
    int rotating = 0; int chompAttack = 1; int shootAttack = 2; int blackHoleAttack = 3;
    //phase 2
    int ramming = 4;

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

    int getHealthState()
    {
        for (int i = 0; i < agent.healthState.Value.Length; i++)
        {
            if (i == agent.healthState.Value.Length - 1) return i;

            if (agent.health.health > agent.healthState.Value[i] && agent.health.health <= agent.healthState.Value[i + 1])
            {
                return i + 1;
            }
        }
        return 0;
    }

    private void Update()
    {
        //run current state
        curState.Tick();

        if (casting)
            return;

        //health state behaviour
        switch (getHealthState())
        {
            case 0:
                break;
            case 1:
                SecondBehaviour();
                break;
            case 2:
                FirstBehaviour();
                break;
        }

        if (Time.time - specialAttackTimer > specialAttackCooldown) canCastSpecial = true;
    }

    void FirstBehaviour()
    {
        //if player is in front, do bite attack
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 10f, transform.forward, out hit, 15f, playerLayer))
        {
            if (curState != states[chompAttack])
            {
                curState.OnExit();
                curState = states[chompAttack];
                curState.OnEnter();
            }
            return;
        }
        //else if (canCastSpecial)
        //{


        //}
        //select from ranged attacks
        else if (Time.time - actionTimer > actionCooldown)
        {
            if (curState != states[shootAttack])
            {
                curState.OnExit();
                curState = states[shootAttack];
                curState.OnEnter();
            }
            //else if(curState != states[blackHoleAttack])
            //{
            //    curState.OnExit();
            //    curState = states[blackHoleAttack];
            //    curState.OnEnter();
            //}
            return;
        }


        //normal movement
        if (curState != states[rotating])
        {
            curState.OnExit();
            curState = states[rotating];
            curState.OnEnter();
        }
    }

    void SecondBehaviour()
    {





        //normal movement
        if (curState != states[ramming])
        {
            curState.OnExit();
            curState = states[ramming];
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
