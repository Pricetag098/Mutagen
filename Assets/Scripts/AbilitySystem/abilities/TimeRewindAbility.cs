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
    [SerializeField] int step = 3;
    [SerializeField] VfxSpawnRequest vfx;
    [SerializeField] GameObject bodyPrefab;
    GameObject body;
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
    bool reversing;
    int index;

    List<TimeData> positionHistory = new List<TimeData>();
    protected override void DoCast(CastData data)
    {
        if (!timer.complete || reversing)
            return;
        vfx.Play(data.origin,data.aimDirection);
        reversing = true;
        index = positionHistory.Count;

        if (OnCast != null)
            OnCast(data);
    }

    

    public override void FixedTick()
    {
		if (reversing)
		{
            index -= step;
            if(index <= 0)
			{
                positionHistory.Clear();
                positionHistory.Add(new TimeData(caster.transform.position, Time.time));
                timer.Reset();
                reversing = false;
				body.SetActive(false);
				lineRenderer.enabled = false;
                FinishCast();
			}
            else
            caster.transform.position = positionHistory[index].position;
            
		}
		else
		{
			
			positionHistory.Add(new TimeData(caster.transform.position,Time.time));
            while (positionHistory[0].time < Time.time - maxTime && positionHistory.Count > 0)
            {
                positionHistory.RemoveAt(0);
            }
            body.transform.position = positionHistory[0].position;
            if(positionHistory.Count > 1)
            {
                Vector3 dir = positionHistory[1].position - positionHistory[0].position;
                dir.y = 0;
                if(dir != Vector3.zero)
                {
					body.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

				}
                

                
            }
		}
        
        

    }

    public override void OnTick()
    {
        timer.Tick();
		if (linePrefab.Enabled)
		{
            lineRenderer.positionCount = positionHistory.Count;
            for(int i = 0; i < positionHistory.Count; i++)
			{
                lineRenderer.SetPosition(i, positionHistory[positionHistory.Count - i -1].position + lineOffset);
			}
			if (timer.complete)
			{
				body.SetActive(true);
                lineRenderer.enabled = true;
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
        body = Instantiate(bodyPrefab);

	}
	protected override void OnUnEquip(Ability replacement)
	{
        if (linePrefab.Enabled)
            Destroy(lineRenderer.gameObject);
        Destroy(body.gameObject);
    }
}
