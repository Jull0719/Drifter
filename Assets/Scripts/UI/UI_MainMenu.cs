using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private UI_Options optionsUI;

    private void Awake()
    {
        optionsUI = transform.root.GetComponentInChildren<UI_Options>(true);        
    }

    private void Start()
    {
        transform.root.GetComponentInChildren<UI_Options>(true).LoadVolume();
        AudioManager.instance.StartBGM("MainMenu_BGM");
        transform.root.GetComponentInChildren<UI_FadeScreen>().FadeIn();
    }

    public void StartNewGameBTN()
    {
        SaveManager.instance.DeleteSaveData();

        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        GameManager.instance.ChangeScene("Level1_Town", WaypointType.None);
    }

    public void ContinueGameBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        GameManager.instance.ContinueGame();
    }

    public void OpenOptionBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
        optionsUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitGameBTN()
    {
        AudioManager.instance.PlayGlobalSfx(Settings.ButtonSFXName);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
