using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{
    public Ability[] abilities;
    public Optional<Animator> animator;
    public Health ownerHealth;
    new public Optional<Rigidbody> rigidbody;

    Optional<PlayerStats> playerStats;
    const string baseAbilityPath = "Abilities/Empty";
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < abilities.Length; i++)
		{
            if(abilities[i] == null)
			{
                abilities[i] = Instantiate(Resources.Load<Ability>(baseAbilityPath));
            }
			else
			{
                abilities[i] = Instantiate(abilities[i]);
            }
            abilities[i].Equip(this);
            
		}
        PlayerStats tempStats;
        playerStats.Enabled = TryGetComponent(out tempStats);
        playerStats.Value = tempStats;
    }

    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Tick();
        }
        
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].FixedTick();
        }
    }

    public void ChangeSpeed(float multiplierChange)
	{
		if (playerStats.Enabled)
		{
            playerStats.Value.speedMulti += multiplierChange;
		}
        //do ai stuff
	}
    public virtual void CastAbility(int index,Ability.CastData castData)
    {
        //Debug.Log(abilities[index].name);
        abilities[index].Cast(castData);
    }

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
            return;
        for (int i = 0; i < abilities.Length; i++)
        { 
            abilities[i].OnDrawGizmos();
        }

     }
}
