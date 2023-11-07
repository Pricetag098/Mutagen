using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsHealth : MonoBehaviour
{
    [SerializeField] Health parentHealth;
    [SerializeField] int maxPartHealth;
    int curPartHealth;


    private void Start()
    {
        curPartHealth = maxPartHealth;
    }



}
