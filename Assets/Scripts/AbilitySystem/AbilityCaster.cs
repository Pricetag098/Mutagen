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

    //cast stats
    public bool castBool = true;
    float castTimer;
    float lastCastDisable;

    Optional<PlayerStats> playerStats;
    const string baseAbilityPath = "Abilities/Empty";
    [HideInInspector]public Ability baseAbility;
    // Start is called before the first frame update
    private void Start()
    {

    }
    void Awake()
    {
        baseAbility = Resources.Load<Ability>(baseAbilityPath);
        for (int i = 0; i < abilities.Length; i++)
		{
            if(abilities[i] == null)
			{
                abilities[i] = Instantiate(baseAbility);
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

    public void SetAbility(Ability ability,int index,bool spawnPickup = true)
	{
        abilities[index].UnEquip(ability);
        abilities[index] = ability;
        ability.Equip(this);
    }

    public void DisableCast(float disableTime)
    {
        if (!castBool)
            return;

        castTimer = Time.time;
        lastCastDisable = disableTime;
        castBool = false;
    }

    public bool canCast()
    {
        if (castBool)
            return true;

        if(Time.time - castTimer > lastCastDisable)
        {
            castBool = true;
            return true;
        }
        return false;
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

    public void SetAllAbilities(Ability[] abilitysToSet)
	{
        for(int i = 0; i < abilities.Length && i < abilitysToSet.Length; i++)
		{
            SetAbility(abilitysToSet[i], i,false);
		}
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

    public void UpdateDirection(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        if (rigidbody.Enabled)
        {
            rigidbody.Value.GetComponent<PlayerMovement>().body.GetComponent<SecondOrderFacer>().targetVec = direction;
        }
        else
        {

            //Jason do stuff here pls
        }
    }
}
