using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PipeColourChanger : MonoBehaviour
{
    public Renderer[] pipesMat;

    [Tooltip("Make attacking matt last")]
    public Material[] materials;
    


    

    public void Change(int index)
    {
        for (int i = 0; i < pipesMat.Length; i++)
        {
            pipesMat[i].material = materials[index];
        }
    }
}
