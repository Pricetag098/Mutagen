using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum hazardType
{
    Reset,
    Slow
}

public class EnviromentalHazard : MonoBehaviour
{
    public DamageData data;
    public hazardType type;
    public float speedMulti;

    
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pm;
        if(other.transform.parent.TryGetComponent(out pm))
        {
            if(type == hazardType.Reset)
            {
                pm.ResetPos();
                data.target = pm.gameObject;
                pm.GetComponent<Health>().TakeDmg(data);
            }
            else
            {
                other.transform.parent.GetComponent<PlayerStats>().speedMulti = speedMulti;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type != hazardType.Slow)
            return;
        PlayerStats stats;
        if(other.transform.parent.TryGetComponent(out stats))
        {
            other.transform.parent.GetComponent<PlayerStats>().speedMulti = 1;
        }
    }
}
