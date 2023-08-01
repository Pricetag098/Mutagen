using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnKill : MonoBehaviour
{
    public GameObject target;
    Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += DestroyObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyObject()
	{
        Destroy(target);
	}
}
