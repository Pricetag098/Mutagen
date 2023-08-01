using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Rewind")]
public class TimeRewindAbility : Ability
{
    [SerializeField] float maxTime;
    [SerializeField] float coolDown;
    float timer;
    struct TimeData 
    { 
        public Vector3 position;
        public float time;
        public TimeData(Vector3 p,float t)
        {
            position = p;
            time = t;
        }
    }


    List<TimeData> positionHistory = new List<TimeData>();
    public override void Cast(CastData data)
    {
        if (timer < coolDown)
            return;
        caster.transform.position = positionHistory[0].position;
        positionHistory.Clear();
        positionHistory.Add(new TimeData(caster.transform.position, Time.time));
        timer = 0;
    }

    

    public override void FixedTick()
    {
        
        positionHistory.Add(new TimeData(caster.transform.position,Time.time));
        while (positionHistory[0].time < Time.time - maxTime && positionHistory.Count > 0)
        {
            positionHistory.RemoveAt(0);
        }
        

    }

    public override void Tick()
    {
        timer += Time.deltaTime;
    }

    public override float GetCoolDownPercent()
    {
        return 1- Mathf.Clamp01(timer / coolDown);
    }

}
