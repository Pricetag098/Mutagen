using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
public class MapManager : MonoBehaviour
{
    static MapManager instance;

    AsyncOperation operation;
    [SerializeField] CanvasGroup group;
    [SerializeField] Image splashImage;
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] InputActionProperty inputAction;
    [SerializeField] float fadeTime = 3;
    public MapPath path;
    bool loadingMap;
    public int mapTeir = 0;
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
        }
        
    }

    private void Update()
    {
        if(loadingMap)
        {
            group.alpha += Time.unscaledDeltaTime / fadeTime;
            
        }
        else
        {
            group.alpha -= Time.unscaledDeltaTime;
        }
    }

    public static void LoadNext()
    {
        instance.DoLoadNext();
    }

    void DoLoadNext()
    {

    }

    void DoLoadMap(Map map)
    {
        if(operation != null)
        if (!operation.isDone)
            Debug.LogError("Error Loading Two Maps @ once");
        Time.timeScale = 0;
        mapName.text = map.mapName;
        splashImage.sprite = map.splashScreen;
        operation = SceneManager.LoadSceneAsync(map.sceneBuildIndex,LoadSceneMode.Single);
        operation.allowSceneActivation = false;
        loadingMap = true;
        inputAction.action.Enable();
    }
    [ContextMenu("Test")]
    void Test()
    {
        
    }
    [ContextMenu("Load")]
    void FinishLoadingMap()
    {
        if (loadingMap)
        {
            if(operation.progress >= .9f)
            {
                Time.timeScale = 1;
                loadingMap = false;
                operation.allowSceneActivation = true;
                inputAction.action.Disable();
                mapTeir++;
            }
        }
        
    }
    void InputFinishLoading(InputAction.CallbackContext context)
    {
        FinishLoadingMap();
    }

}
