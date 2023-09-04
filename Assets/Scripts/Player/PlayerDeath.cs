using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    MapManager mapManager;
    [SerializeField] Map deathScreen;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Health>().OnDeath += Die;
        mapManager = FindObjectOfType<MapManager>();
    }

    void Die(DamageData data)
    {

    }

}
