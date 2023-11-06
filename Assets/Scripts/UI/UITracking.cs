using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITracking : MonoBehaviour
{
    public Transform body;
    public Health Health;
    // Start is called before the first frame update
    void Start()
    {
        Health.OnDeath += Relocate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Relocate(DamageData damage)
    {
        this.gameObject.transform.position = new Vector3(body.position.x,this.transform.position.y,body.position.z);
    }
}
