using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string bgmName;

    private void Start()
    {
        AudioManager.instance.StartBGM(bgmName);
    }
}
