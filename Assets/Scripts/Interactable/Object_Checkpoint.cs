using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Object_Checkpoint[] checkpoints;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var checkpoint in checkpoints)
            checkpoint.ActivateCheckPoint(false);

        SaveManager.instance.GetGameData().savedCheckPoint = transform.position;
        ActivateCheckPoint(true);
    }

    private void ActivateCheckPoint(bool active)
    {
        animator.SetBool("isActive", active);
    }

    public void SaveData(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckPoint == transform.position;
        ActivateCheckPoint(active);

        if (active)
            Player.Instance.TeleportPlayer(data.savedCheckPoint);
    }
}
