using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeColourChanger : MonoBehaviour
{
    public Renderer[] pipesMat;

    public void Change(Material newMat)
    {
        for (int i = 0; i < pipesMat.Length; i++)
        {
            pipesMat[i].material = newMat;
        }
    }
}
