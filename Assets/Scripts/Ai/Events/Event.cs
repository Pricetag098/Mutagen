using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("AI/Events/Empty"))]
public abstract class Event : ScriptableObject
{
    public bool used;



    public abstract void Play(Enemy agent);
    
}
