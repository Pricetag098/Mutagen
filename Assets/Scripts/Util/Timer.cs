using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float currentTime;
    public float maxTime;
    public bool complete;

    public delegate void Callback();
    public Callback OnComplete;

	public Timer(float time, bool startComplete = true, Callback onComplete = null)
	{

		this.maxTime = time;
		OnComplete = onComplete;
        complete = false;
        this.currentTime = startComplete ? maxTime : 0;
	}

	public void Tick()
	{
		if(complete)
            return;
        if(currentTime >= maxTime)
		{
            complete = true;
            if(OnComplete != null)
                OnComplete();
            return;
		}
        currentTime += Time.deltaTime;

	}

    public void Reset()
	{
        currentTime = 0;
        complete = false;
	}

    public float Progress { get { return Mathf.Clamp01(currentTime / maxTime); }}
}
