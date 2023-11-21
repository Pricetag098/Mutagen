using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxObject : MonoBehaviour
{
    Optional<ParticleSystem> particle;
	Optional<VisualEffect> visualEffect;
	Optional<SoundPlayer> soundPlayer;
	PooledObject pooledObject;
	Transform target;
	bool hasTarget;

	private void Awake()
	{
		particle.Value = GetComponentInChildren<ParticleSystem>();
		soundPlayer.Value = GetComponentInChildren<SoundPlayer>();
		visualEffect.Value = GetComponentInChildren<VisualEffect>();
		particle.Enabled = particle.Value != null;
		soundPlayer.Enabled = soundPlayer.Value != null;
		visualEffect.Enabled = visualEffect.Value != null;
		pooledObject = GetComponent<PooledObject>();
		pooledObject.OnDespawn += OnDespawn;
	}

	private void Update()
	{
		if (hasTarget && target != null)
			transform.position = target.position;
	}

	public void Play()
	{
		if(particle.Enabled)
			particle.Value.Play();
		if(visualEffect.Enabled)
			visualEffect.Value.Play();
		if(soundPlayer.Enabled)
		soundPlayer.Value.Play();
	}
	public void PlayFollow(Transform target)
	{
		this.target = target;
		hasTarget = true;
		Play();
	}

	void OnDespawn()
	{
		hasTarget = false;
		if (particle.Enabled)
			particle.Value.Stop();
		if (soundPlayer.Enabled)
			soundPlayer.Value.Stop();
		if (visualEffect.Enabled)
			visualEffect.Value.Stop();
	}

	

	
}
