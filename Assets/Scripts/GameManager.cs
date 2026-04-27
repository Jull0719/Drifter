using System.Collections;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager instance;
    Vector3 lastPlayerPosition;
    string lastSceneName;

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
        // TODO: Fade In

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(scene);

        // TODO: Fade Out

        yield return new WaitForSeconds(0.2f);

        Player player = Player.Instance;

        if (player == null)
            yield break;

        var position = GetNewPlayerPosition(waypointType);

        if (position != Vector3.zero)
            player.TeleportPlayer(position);
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
                .Where(cp => data.unlockedCheckpointDict[cp.GetCheckpointId()])
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
    }

    public void LoadData(GameData data)
    {
        lastPlayerPosition = data.lastPlayerPosition;
        lastSceneName = data.lastSceneName;

        if (string.IsNullOrEmpty(lastSceneName))
            lastSceneName = "Level1_Town";
    }
}
