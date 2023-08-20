using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelLoading/Tier")]
public class MapTeir : ScriptableObject
{
    public List<Map> maps;
    public Map GetMap()
    {
        return maps[Random.Range(0, maps.Count)];
    }

}
