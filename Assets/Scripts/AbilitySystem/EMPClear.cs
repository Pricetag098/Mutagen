using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPClear : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.layer == targetLayer);
        if(collision.gameObject.layer == targetLayer)
        {
            Debug.Log("Hit");
            Destroy(collision.gameObject);
        }
    }
}
