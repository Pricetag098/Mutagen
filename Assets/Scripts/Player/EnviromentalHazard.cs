using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentalHazard : MonoBehaviour
{
    public DamageData data;
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm;
        if(other.TryGetComponent(out pm))
        {
            pm.ResetPos();
            data.target = pm.gameObject;
            pm.GetComponent<Health>().TakeDmg(data);
        }
    }
}
