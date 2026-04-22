using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] string sceneToTransfer;
    [SerializeField] WaypointType currentWaypointType;
    [SerializeField] WaypointType connectWaypointType;
    bool canBeTriggered = true;

    private void OnValidate()
    {
        gameObject.name = "Object_Waypont - " + currentWaypointType.ToString() + " - " + sceneToTransfer;

        if (currentWaypointType == WaypointType.Entry)
            connectWaypointType = WaypointType.Exit;
        else if (currentWaypointType == WaypointType.Exit)
            connectWaypointType = WaypointType.Entry;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeTriggered)
            return;
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(sceneToTransfer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTriggered = true;
    }
}
