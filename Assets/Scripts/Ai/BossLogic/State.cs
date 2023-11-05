using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{

    public delegate void OnEnter();
    public delegate void Update();
    public delegate void OnExit();

}
