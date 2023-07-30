using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    private void Start()
    {
        tree = tree.Clone();
        tree.Bind(GetComponent<Enemy>());
        //tree.blackboard.agent = this.GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        tree.Update();
    }
}
