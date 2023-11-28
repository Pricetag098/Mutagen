using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pots : MonoBehaviour
{
    public VfxSpawnRequest vfx;

    private void OnTriggerEnter(Collider other)
    {
        vfx.Play(transform.position, transform.forward);
        Destroy(gameObject);
    }
}
