using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public StateManager manager;

    public abstract void OnEnter();
    public abstract void Tick();
    public abstract void OnExit();

}
