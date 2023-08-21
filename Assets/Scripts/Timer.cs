using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float time;
    public float maxTime;
    public bool complete;

    public delegate void Callback();
    public Callback OnComplete;

	public Timer(float time, bool startComplete = true, Callback onComplete = null)
	{

		this.maxTime = time;
		OnComplete = onComplete;
        complete = false;
        this.time = startComplete ? maxTime : 0;
	}

	public void Tick()
	{
		if(complete)
            return;
        if(time >= maxTime)
		{
            complete = true;
            if(OnComplete != null)
                OnComplete();
            return;
		}
        time += Time.deltaTime;

	}

    public void Reset()
	{
        time = 0;
        complete = false;
	}

    public float Progress { get { return Mathf.Clamp01(time / maxTime); }}
}
