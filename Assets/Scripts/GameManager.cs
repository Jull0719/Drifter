using System.Collections;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    private Vector3 lastPlayerPosition;
    private string lastSceneName;
    private bool dataLoaded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ContinueGame()
    {
        ChangeScene(lastSceneName, WaypointType.None);
    }

    public void ReStart()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, WaypointType.None);
    }

    //public Vector3 SetPlayerPosition(Vector3 position) => lastPlayerPosition = position;

    public void ChangeScene(string sceneToTransfer, WaypointType waypointType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneToTransfer, waypointType));
    }

    IEnumerator ChangeSceneCo(string scene, WaypointType waypointType)
    {
        var fadeUI = FindFadeScreen();

        fadeUI.FadeOut();
        yield return fadeUI.fadeCo;

        SceneManager.LoadScene(scene);

        dataLoaded = false; // 当数据加载完成时，标记为true
        yield return null;

        while (dataLoaded == false)
        {
            yield return null;
        }

        fadeUI = FindFadeScreen();
        fadeUI.FadeIn();

        Player player = Player.Instance;

        if (player == null)
            yield break;

        var position = GetNewPlayerPosition(waypointType);

        if (position != Vector3.zero)
            player.TeleportPlayer(position);
    }

    private UI_FadeScreen FindFadeScreen()
    {
        if (UI.instance != null)
            return UI.instance.fadeUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
    }

    private Vector3 GetNewPlayerPosition(WaypointType type)
    {
        if (type == WaypointType.None)
        {
            var data = SaveManager.instance.GetGameData();

            var waypoints = GameObject.FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == WaypointType.Entry)
                .Select(wp => wp.GetRespawnPosition())
                .ToList();

            var checkpoints = GameObject.FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None)
                .Where(cp => data.unlockedCheckpointDict.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetRespawnPosition())
                .ToList();

            var positions = waypoints.Concat(checkpoints).ToList();

            if (positions.Count() == 0)
                return Vector3.zero;

            return positions
                .OrderBy(pos => Vector3.Distance(lastPlayerPosition, pos))
                .First();
        }

        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(WaypointType type)
    {
        var wayPoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in wayPoints)
        {
            if (waypoint.GetWaypointType() == type)
            {
                return waypoint.GetRespawnPosition();
            }
        }

        return Vector3.zero;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu")
            return;

        data.lastPlayerPosition = Player.Instance.transform.position;
        data.lastSceneName = currentScene;
        dataLoaded = false;
    }

    public void LoadData(GameData data)
    {
        lastPlayerPosition = data.lastPlayerPosition;
        lastSceneName = data.lastSceneName;

        if (string.IsNullOrEmpty(lastSceneName))
            lastSceneName = "Level1_Town";

        dataLoaded = true;
    }
}
