using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextTarget : MonoBehaviour
{
    public Health health;
    [HideInInspector] public FloatingTextManager textManager;

    private void Awake()
    {
        textManager = FindObjectOfType<FloatingTextManager>();
    }

    private void Start()
    {
        if(health == null)
        health = GetComponent<Health>();
        health.OnHit += OnHit;
    }

    void OnHit(DamageData data)
    {
        data.target = this.gameObject;
        textManager.Show(data);
    }
}
