using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using DG.Tweening;

public class MapManager : MonoBehaviour
{
    static MapManager instance;

    AsyncOperation operation;
    [SerializeField] CanvasGroup group;
    [SerializeField] Image splashImage;
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] InputActionProperty inputAction;
    [SerializeField] float fadeTime = 3,waitTime = 1;
    public MapPath path;
    bool loadingMap;
    public int mapTeir = 0;

    //resetting
    public PlayerData playerData = null;
    public Loadout defaultLoadout;


    private void Awake()
    {
        mapTeir = 0;
        if(instance != null)
        {
            Destroy(gameObject);
            Debug.LogError("Two Map managers loaded at once?");
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            inputAction.action.performed += InputFinishLoading;
            SceneManager.sceneLoaded += LevelLoaded;
            playerData = null;
        }
    }

    public static void ResetProgress()
    {
        instance.mapTeir = 0;
        instance.playerData = null;

        
    }

    void LevelLoaded(Scene scene, LoadSceneMode mode)
	{
        //loadingMap = false;
        Sequence s = DOTween.Sequence(this);
        
        
        //s.AppendCallback(() => { operation.allowSceneActivation = true; });
        //s.AppendInterval(waitTime);
        s.Append(group.DOFade(0, fadeTime));
    }

    private void Update()
    {
        
    }

    public static void LoadNext()
    {
        instance.DoLoadNext();
    }

    void DoLoadNext()
    {
        if(loadingMap)
            return;
        if(path.teirs.Count <= mapTeir)
		{
            mapTeir = 0;
		}
        DoLoadMap(path.teirs[mapTeir].GetMap());
        mapTeir++;
    }

    public static void LoadMap(Map map)
    {
        instance.DoLoadMap(map);
    }
    Map mapToLoad;
    void DoLoadMap(Map map)
    {
        if(operation != null)
        if (!operation.isDone)
            Debug.LogError("Error Loading Two Maps @ once");
        Time.timeScale = 0;
        loadingMap = true;
        mapName.text = map.mapName;
        splashImage.sprite = map.splashScreen;
        
        Sequence s = DOTween.Sequence(this);
        s.Append(group.DOFade(1, fadeTime));
        s.AppendCallback(() => {
            operation = SceneManager.LoadSceneAsync(map.sceneBuildIndex, LoadSceneMode.Single);
            operation.allowSceneActivation = false;
        });
        s.AppendInterval(waitTime);
        s.AppendCallback(() => { operation.allowSceneActivation = true; loadingMap = false; });
        
        
        mapToLoad = map;

        
        
        
    }
    [ContextMenu("Test")]
    void Test()
    {
        DoLoadNext();
    }

    public static void SetupPlayer(GameObject player,Transform entrance)
	{
        
        if(instance.playerData != null)
		{
            instance.playerData.SetupPlayer(player,entrance);
		}
	}
    public static void SavePlayer(GameObject player,Transform exit)
	{
        instance.playerData = new PlayerData(player,exit);
	}

    [ContextMenu("Load")]
    void FinishLoadingMap()
    {
        if (loadingMap)
        {
            if(operation.progress >= .9f)
            {
                Time.timeScale = 1;
                //loadingMap = false;
                operation.allowSceneActivation = true;
                inputAction.action.Disable();


            }
        }
        
    }

    void InputFinishLoading(InputAction.CallbackContext context)
    {
        FinishLoadingMap();
    }

    public class PlayerData
	{
        Ability[] equipedAbilities;
        float health;
        Vector3 spawnPos;
        public void SetupPlayer(GameObject player,Transform spawnPoint)
		{
            Debug.Log(equipedAbilities == null);
            player.GetComponent<PlayerAbilityCaster>().SetAllAbilities(equipedAbilities);
            player.GetComponent<Health>().health = health;
            player.transform.position = spawnPoint.TransformPoint(spawnPos);
        }

        public PlayerData(GameObject player,Transform exit)
		{
            equipedAbilities = player.GetComponent<AbilityCaster>().abilities;
            health = player.GetComponent<Health>().health;
            spawnPos = exit.InverseTransformPoint(player.transform.position);
		}
	} 
}
