using System;
using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    [SerializeField] string checkpointId;
    [SerializeField] Transform respawnPosition;

    Animator animator;
    AudioSource audioSource;

    public bool isActive { get; private set; }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (String.IsNullOrEmpty(checkpointId))
            checkpointId = Guid.NewGuid().ToString();
#endif
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateCheckPoint(true);
    }

    private void ActivateCheckPoint(bool active)
    {
        isActive = active;
        animator.SetBool("isActive", active);

        if (isActive && !audioSource.isPlaying)
            audioSource.Play();
        
        if (!isActive)
            audioSource.Stop();
    }

    public string GetCheckpointId() => checkpointId;
    public Vector3 GetRespawnPosition()
    {
        return respawnPosition == null ? transform.position : respawnPosition.position;
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false)
            return;

        if (!data.unlockedCheckpointDict.ContainsKey(checkpointId))
            data.unlockedCheckpointDict.Add(checkpointId, true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpointDict.TryGetValue(checkpointId, out active);
        ActivateCheckPoint(true);
    }
}
