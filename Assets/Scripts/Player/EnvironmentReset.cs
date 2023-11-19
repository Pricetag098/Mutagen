using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentReset : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm;
        if(other.TryGetComponent<PlayerMovement>(out pm))
        {
            //pm.lastSafeLocation = transform.position;
        }
    }
}
