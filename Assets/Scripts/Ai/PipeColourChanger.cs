using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PipeColourChanger : MonoBehaviour
{
    public Renderer[] pipesMat;
    [Tooltip("Make attacking matt last")]
    public Material[] materials;
    Material defaultMaterial;

    private void Start()
    {
        defaultMaterial = pipesMat[0].material;
    }

    public void Default()
    {
        for (int i = 0; i < pipesMat.Length; i++)
        {
            pipesMat[i].material = defaultMaterial;
        }
    }

    public void Change(int index)
    {
        for (int i = 0; i < pipesMat.Length; i++)
        {
            pipesMat[i].material = materials[index];
        }
    }
}
