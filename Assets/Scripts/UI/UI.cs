using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;

    public PlayerInputSet input;

    public UI_PlayerStats statUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Shop shopUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }

    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    public UI_Dialogue dialogueUI { get; private set; }

    private bool inventoryUIEnabled;
    private bool statUIEnabled;

    [SerializeField] private TextMeshProUGUI warningText;
    private Coroutine blinkCo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        statUI = GetComponentInChildren<UI_PlayerStats>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);

        storageUI = GetComponentInChildren<UI_Storage>(true);
        shopUI = GetComponentInChildren<UI_Shop>(true);

        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        dialogueUI = GetComponentInChildren<UI_Dialogue>(true);
    }

    // 设置UI面板的按键
    public void SetupUIControls(PlayerInputSet inputSet)
    {
        input = inputSet;

        // UI快捷键
        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();
        input.UI.StatUI.performed += ctx => ToggleStatUI();
        input.UI.OptionsUI.performed += ctx =>
        {
            foreach (var element in uiElements)
            {
                if (element.activeSelf)
                {
                    SwitchToInGameUI();
                    return;
                }
            }

            ToggleOptionsUI();
        };
    }

    // 禁用或启用角色输入
    public void StopPlayerControls(bool stopControls)
    {
        if (stopControls)
            input.Gameplay.Disable();
        else
            input.Gameplay.Enable();
    }

    // 控制角色输入
    public void StopPlayerControlsIfNeeded()
    {
        foreach (var element in uiElements)
        {
            if (element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }

    // 背包
    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        inventoryUIEnabled = !inventoryUIEnabled;
        inventoryUI.gameObject.SetActive(inventoryUIEnabled);
        HideAllTooltip();

        //StopPlayerControlsIfNeeded();
    }

    // 属性
    public void ToggleStatUI()
    {
        statUI.transform.SetAsLastSibling();
        SetTooltipsAsLastSibling();

        statUIEnabled = !statUIEnabled;
        statUI.gameObject.SetActive(statUIEnabled);
        HideAllTooltip();

        //StopPlayerControlsIfNeeded();
    }

    // 选项菜单
    public void ToggleOptionsUI()
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        HideAllTooltip();
        StopPlayerControls(true);
        optionsUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    // 返回游戏
    public void SwitchToInGameUI()
    {
        foreach (var element in uiElements)
            element.SetActive(false);

        HideAllTooltip();
        StopPlayerControls(false);
        inGameUI.gameObject.SetActive(true);
        Time.timeScale = 1;

        inventoryUIEnabled = false;
        statUIEnabled = false;
    }

    // 打开仓库
    public void OpenStorageUI(bool isOpen)
    {
        storageUI.gameObject.SetActive(isOpen);

        StopPlayerControls(isOpen);

        if (isOpen == false)
            HideAllTooltip();
    }

    // 打开商店
    public void OpenShopUI(bool isOpen)
    {
        shopUI.gameObject.SetActive(isOpen);

        StopPlayerControls(isOpen);

        if (isOpen == false)
            HideAllTooltip();
    }

    // 关闭ToolTip
    public void HideAllTooltip()
    {
        itemToolTip.ShowToolTip(false, null);
        statToolTip.ShowToolTip(false, null);
    }

    // 将ToolTip置于最上方
    public void SetTooltipsAsLastSibling()
    {
        itemToolTip.transform.SetAsLastSibling();
        statToolTip.transform.SetAsLastSibling();
    }

    #region 文字提示

    public void SetWarningText(string text, bool isBlink)
    {
        warningText.text = text;

        if (isBlink)
            BlinkEffect();
    }

    // Blink Effect
    public void BlinkEffect()
    {
        if (blinkCo != null)
            StopCoroutine(blinkCo);

        blinkCo = StartCoroutine(BlinkCo(0.15f, 2));
    }

    IEnumerator BlinkCo(float blinkInterval, int blinkAmount)
    {
        for (int i = 0; i < blinkAmount; i++)
        {
            warningText.color = Color.red;
            yield return new WaitForSeconds(blinkInterval);
            warningText.color = Color.white;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    #endregion
}
