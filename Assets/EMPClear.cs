using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPClear : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == targetLayer)
        {
            Debug.Log("Hit");
            Destroy(collision.gameObject);
        }
    }
}
