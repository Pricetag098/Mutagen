using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public NavMeshAgent agent;
    [SerializeField] LayerMask playerLayer;
    public Transform castOrigin;
    public Transform movementPoint;
    [HideInInspector] public Transform movementTarget;

    [Header("States")]
    public State[] states;
    Optional<State> curState;
    public bool casting;

    //readability
    int rotating = 0; int chompAttack = 1;




    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        foreach(State state in states)
        {
            state.manager = this;
        }
    }

    private void Update()
    {
        if(curState.Enabled)
        curState.Value.Tick();

        RaycastHit hit;
        Vector3 cast = transform.position + Vector3.forward * 50;
        cast.y = transform.position.y + 10;
        if(Physics.SphereCast(castOrigin.position, 50f, cast, out hit, 101f, playerLayer))
        {
            Debug.Log("Hit");
        }




        
        if(curState.Value != states[rotating])
        {
            if (curState.Enabled)
                curState.Value.OnExit();
            else
                curState.Enabled = true;
            curState.Value = states[rotating];
            curState.Value.OnEnter(); 
        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 draw = transform.position + Vector3.forward * 50;
        draw.y = transform.position.y;
        Gizmos.DrawLine(transform.position, draw);
    }
}
