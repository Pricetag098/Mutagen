using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : ScriptableObject
{
    public int sceneBuildIndex = 0;
   

    public void Load()
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
