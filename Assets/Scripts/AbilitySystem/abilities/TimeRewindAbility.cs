using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Abilities/Rewind")]
public class TimeRewindAbility : Ability
{
    [SerializeField] float maxTime;
    [SerializeField] float coolDown;
    Timer timer;
    LineRenderer lineRenderer;
    [SerializeField] Optional<GameObject> linePrefab;
    [SerializeField] Vector3 lineOffset;
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
    protected override void DoCast(CastData data)
    {
        if (!timer.complete)
            return;
        caster.transform.position = positionHistory[0].position;
        positionHistory.Clear();
        positionHistory.Add(new TimeData(caster.transform.position, Time.time));
        timer.Reset();
        if (OnCast != null)
            OnCast(data);
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
        timer.Tick();
		if (linePrefab.Enabled)
		{
            lineRenderer.positionCount = positionHistory.Count;
            for(int i = 0; i < positionHistory.Count; i++)
			{
                lineRenderer.SetPosition(i, positionHistory[positionHistory.Count - i -1].position + lineOffset);
			}
		}
    }

    public override float GetCoolDownPercent()
    {
        return timer.Progress;
    }
	protected override void OnEquip()
	{
		if(linePrefab.Enabled)
            lineRenderer = Instantiate(linePrefab.Value).GetComponent<LineRenderer>();
        timer = new Timer(coolDown);
	}
	protected override void OnUnEquip(Ability replacement)
	{
        if (linePrefab.Enabled)
            Destroy(lineRenderer.gameObject);
    }
}
