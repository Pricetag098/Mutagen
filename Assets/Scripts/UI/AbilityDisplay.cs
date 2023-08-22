using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDisplay : MonoBehaviour
{
    AbilityIcon[] abilityIcons;
    // Start is called before the first frame update
    void Awake()
    {
        abilityIcons = GetComponentsInChildren<AbilityIcon>();
    }

    public void UpdateUI(Ability[] abilities)
	{
        for(int i = 0; i < abilities.Length; i++)
		{
            abilityIcons[i].SetAbility(abilities[i]);
		}
	}
}
