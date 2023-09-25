using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/Empty"))]
public abstract class Event : ScriptableObject
{
    [HideInInspector] public bool used;
    public abstract void Play(Enemy agent);
    
}
