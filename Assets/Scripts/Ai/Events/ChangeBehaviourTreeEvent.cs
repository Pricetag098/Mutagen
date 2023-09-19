using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/ChangeBehaviourTree"))]
public class ChangeBehaviourTreeEvent : Event
{
    public BehaviourTree newTree;

    public override void Play(Enemy agent)
    {
        used = true;
        agent.behaviourTree.tree = newTree.Clone();
    }
}
