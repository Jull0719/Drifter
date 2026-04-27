using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private bool shouldEncrypt = false;
    private string fileName = "DrifterData.json";

    private List<ISaveable> allSaveables;
    private GameData gameData;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, shouldEncrypt);
        allSaveables = FindAllSaveables();
        yield return new WaitForSeconds(0.01f);

        LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SaveGame();
        if (Input.GetKeyDown(KeyCode.O))
            LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public GameData GetGameData() => gameData;

    private List<ISaveable> FindAllSaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
   .OfType<ISaveable>().ToList();
    }

    public void SaveGame()
    {
        //UI.instance.SetWarningText("已储存游戏", false);

        foreach (var saveable in allSaveables)
            saveable.SaveData(ref gameData);

        fileDataHandler.SaveData(gameData);

        Debug.Log("数据已储存");
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("创建游戏数据");
            gameData = new GameData();
            return;
        }

        foreach (var saveable in allSaveables)
            saveable.LoadData(gameData);

        //UI.instance.SetWarningText("已加载游戏", false);
        Debug.Log("数据已加载");
    }

    [ContextMenu("删除游戏数据")]
    public void DeleteSaveData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, shouldEncrypt);
        fileDataHandler.DeleteData();
    }
}
