using UnityEngine;

public class Object_Waypoint : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneToTransfer;
    [SerializeField] WaypointType currentWaypointType;
    [SerializeField] WaypointType connectWaypointType;
    [SerializeField] Transform respawnPoint;
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

        GameManager.instance.ChangeScene(sceneToTransfer, connectWaypointType);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetTriggered(true);
    }

    public WaypointType GetWaypointType() => currentWaypointType;
    public void SetTriggered(bool canBeTriggered) => this.canBeTriggered = canBeTriggered;
    public Vector3 GetRespawnPosition()
    {
        canBeTriggered = false;
        return respawnPoint == null ? transform.position : respawnPoint.position;
    }

    public void Interact()
    {
        GameManager.instance.ChangeScene(sceneToTransfer, connectWaypointType);
    }
}
