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
    

    const string baseAbilityPath = "Abilities/Empty";
    [HideInInspector]public Ability baseAbility;
    // Start is called before the first frame update
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
    }
    public void SetAbility(Ability ability,int index)
	{
        ability = Instantiate(ability);
        abilities[index].UnEquip(ability);
		if (abilities[index].pickupPrefab.Enabled)
		{
            GameObject pickup = Instantiate(abilities[index].pickupPrefab.Value);
            pickup.transform.position = transform.position;
		}
        abilities[index] = ability;
        ability.Equip(this);

    }
    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Tick();
        }
        
    }


    public virtual void CastAbility(int index,Ability.CastData castData)
    {
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
