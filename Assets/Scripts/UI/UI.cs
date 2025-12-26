using System.Collections;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    [SerializeField] private TextMeshProUGUI warningText;
    private Coroutine blinkCo;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statToolTip = GetComponentInChildren<UI_StatToolTip>();
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
}
