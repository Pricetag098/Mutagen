using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/Empty"))]
public class Event : ScriptableObject
{
    public bool used;

    public virtual void Play(Enemy agent)
    {
        used = true;
    }
}
