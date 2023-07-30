using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SetterNode : Node
{
    [HideInInspector] public Node child;

    public override Node Clone()
    {
        SetterNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }
}
