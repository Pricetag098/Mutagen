using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/ChangeLoadout"))]
public class ChangeLoadoutEvent : Event
{
    public Loadout newLoadout;

    public override void Play(Enemy agent)
    {
        used = true;
        agent.caster.AssignLoadout(newLoadout);
    }
}
