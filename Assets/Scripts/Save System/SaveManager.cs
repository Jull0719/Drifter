using System.Collections;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private bool shouldEncrypt = false;
    private string fileName = "DrifterData.json";

    private ISaveable[] saveables;
    private GameData gameData;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
    }

    private IEnumerator Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, shouldEncrypt);
        saveables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
    .OfType<ISaveable>().ToArray();

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

    public void SaveGame()
    {
        UI.instance.SetWarningText("已储存游戏", false);

        foreach (var saveable in saveables)
            saveable.SaveData(ref gameData);

        fileDataHandler.SaveData(gameData);
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

        foreach (var saveable in saveables)
            saveable.LoadData(gameData);

        UI.instance.SetWarningText("已加载游戏", false);
    }

    [ContextMenu("删除游戏数据")]
    public void DeleteSaveData()
    {
        //fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, shouldEncrypt);
        fileDataHandler.DeleteData();
    }
}
