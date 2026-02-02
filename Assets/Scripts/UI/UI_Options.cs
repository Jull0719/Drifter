using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle healthBarToggle;

    private Player player;
    private UI_MiniHealthBar healthBar;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBar = player.GetComponentInChildren<UI_MiniHealthBar>(true);
        healthBarToggle.onValueChanged.AddListener(ToggleMiniHealthBar);
    }

    // 关闭/开启血条
    public void ToggleMiniHealthBar(bool enabled) => healthBar.gameObject.SetActive(enabled);
}
