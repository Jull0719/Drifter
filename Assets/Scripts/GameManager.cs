using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    public void ChangeScene(string sceneToTransfer, WaypointType waypointType)
    {
        StartCoroutine(ChangeSceneCo(sceneToTransfer, waypointType));
    }

    IEnumerator ChangeSceneCo(string scene, WaypointType waypointType)
    {
        // TODO: Fade In

        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(scene);

        // TODO: Fade Out

        yield return new WaitForSeconds(0.2f);

        var position = GetPosition(waypointType);

        if (position != Vector3.zero)
            Player.Instance.TeleportPlayer(position);
    }

    private Vector3 GetPosition(WaypointType type)
    {
        var wayPoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);
        foreach (var waypoint in wayPoints)
        {
            if (waypoint.GetWaypointType() == type)
            {
                waypoint.SetTriggered(false);
                return waypoint.GetRespawnPosition();
            }
        }

        return Vector3.zero;
    }
}
