using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pots : MonoBehaviour
{
    public VfxSpawnRequest vfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        vfx.Play(transform.position, transform.forward);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
