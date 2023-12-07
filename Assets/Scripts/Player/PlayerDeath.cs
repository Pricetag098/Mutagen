using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] Map deathScreen;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Health>().OnDeath += Die;

    }

    void Die(DamageData data)
    {
        //GetComponent<PlayerAbilityCaster>().SetAllAbilities(null);
        MapManager.ResetProgress();
        MapManager.LoadMap(deathScreen);
    }
}
