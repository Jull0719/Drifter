using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void StartNewGameBTN()
    {
        Debug.Log("开始游戏");
    }

    public void QuitGameBTN()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
