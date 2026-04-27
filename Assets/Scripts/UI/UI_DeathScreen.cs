using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void ReturnToCheckpoint()
    {
        GameManager.instance.ReStart();
    }

    public void ReturnToTown()
    {
        GameManager.instance.ChangeScene("Level1_Town",WaypointType.None);
    }

    public void ReturnToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", WaypointType.None);
    }
}
