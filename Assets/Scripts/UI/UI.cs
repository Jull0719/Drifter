using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_Inventory inventoryUI { get; private set; }
    public UI_PlayerStats statUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Shop shopUI { get; private set; }

    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    public UI_Dialogue dialogueUI { get; private set; }

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
        storageUI = GetComponentInChildren<UI_Storage>(true);
        shopUI = GetComponentInChildren<UI_Shop>(true);

        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();

        dialogueUI = GetComponentInChildren<UI_Dialogue>(true);
    }

    public void StartButton()
    {
        Debug.Log("开始游戏");
        // 加载场景1

    }

    public void QuitButton()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }

    // 关闭或开启指定UI面板
    public void ToggleUI(GameObject uiObject) => uiObject.SetActive(!uiObject.activeSelf);

    // 背包
    public void OpenInventoryUI()
    {
        ToggleUI(inventoryUI.gameObject);
        itemToolTip.ShowToolTip(false, null);
    }

    // 属性
    public void ToggleStatUI()
    {
        ToggleUI(statUI.gameObject);
        statToolTip.ShowToolTip(false, null);
    }

    // 打开仓库
    public void OpenStorageUI()
    {
        storageUI.gameObject.SetActive(true);
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
