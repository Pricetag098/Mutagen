using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelLoading/MapPath")]
public class MapPath : ScriptableObject
{
    public List<MapTeir> teirs;

}
