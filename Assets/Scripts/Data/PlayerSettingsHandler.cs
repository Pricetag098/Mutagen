using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;

public class PlayerSettingsHandler : MonoBehaviour
{
    public static PlayerSettingsHandler instance { get; private set; }
    [SerializeField] string fileName = "Data.dat";
    [SerializeField] bool useEncryption = false;
    SettingsData settingsData;
    FileDataHandler<SettingsData> fileHandler;
    List<IDataPersistance<SettingsData>> persistanceObjects;
    private void Awake()
    {


        if (instance != null)
        {
            Debug.LogError("cannot have two eventsystems at once");
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        persistanceObjects = FindAllDataPersistance();

        fileHandler = new FileDataHandler<SettingsData>(Application.persistentDataPath, fileName, useEncryption);
        LoadGame();
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        settingsData = new SettingsData();
    }
    [ContextMenu("Open Thing")]
    void OpenThing()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
    public void SaveGame()
    {
        foreach (IDataPersistance<SettingsData> persistanceObject in persistanceObjects)
        {
            persistanceObject.SaveData(ref settingsData);
        }

        fileHandler.Save(settingsData);
    }

    public void LoadGame()
    {
        settingsData = fileHandler.Load();

        if (settingsData == null)
        {
            NewGame();
        }

        foreach (IDataPersistance<SettingsData> persistanceObject in persistanceObjects)
        {
            persistanceObject.LoadData(settingsData);
        }

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    List<IDataPersistance<SettingsData>> FindAllDataPersistance()
    {
        IEnumerable<IDataPersistance<SettingsData>> list = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance<SettingsData>>();
        return new List<IDataPersistance<SettingsData>>(list);
    }

}