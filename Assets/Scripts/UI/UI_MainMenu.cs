using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().FadeIn();       
    }

    public void StartNewGameBTN()
    {
        SaveManager.instance.DeleteSaveData();
        GameManager.instance.ChangeScene("Level1_Town", WaypointType.None);
    }

    public void ContinueGameBTN()
    {
        GameManager.instance.ContinueGame();
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
