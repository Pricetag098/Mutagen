using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/DebugEvent"))]
public class DebugEvent : Event
{
    public override void Play(Enemy agent)
    {
        used = true;
        Debug.Log("Played event");
    }
}
