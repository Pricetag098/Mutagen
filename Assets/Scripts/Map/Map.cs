using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="LevelLoading/Map")]
public class Map : ScriptableObject
{
    public int sceneBuildIndex = 0;
    public string mapName;
    public Sprite splashScreen;

}
