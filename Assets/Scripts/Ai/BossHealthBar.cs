using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    Enemy agent;

    private void Start()
    {
        agent = GetComponent<Enemy>();
        agent.onActivate += OnActivate;
    }

    void OnActivate()
    {
        Debug.Log("Activate");
    }

}
