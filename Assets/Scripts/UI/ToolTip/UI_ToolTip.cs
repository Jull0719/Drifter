using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    protected RectTransform rect;
    private Vector3 hidePosition = new Vector3(9999, 9999, 0);

    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        transform.position = hidePosition;
    }

    public virtual void ShowToolTip(bool isShow, RectTransform taregt)
    {
        if (isShow)
            UpdatePosition(taregt);
        else
            transform.position = hidePosition;
    }

    // 更新ToolTip显示的位置
    private void UpdatePosition(RectTransform target)
    {
        // 屏幕宽高
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // 提示框宽高
        float toolTipWidth = rect.sizeDelta.x;
        float toolTipHeight = rect.sizeDelta.y;

        // 目标对象宽高
        float targetWidth = target.sizeDelta.x;
        float targetHeight = target.sizeDelta.y;

        // 最开始时将提示框置于目标对象位置
        Vector2 targetPos = target.position;

        // 当ToolTip超出屏幕右侧时，左移
        if (targetPos.x + toolTipWidth + targetWidth / 2 > screenWidth)
            targetPos.x = targetPos.x - (toolTipWidth + targetWidth) / 2f - xOffset;
        else // 默认在target右侧
            targetPos.x = targetPos.x + (toolTipWidth + targetWidth) / 2f + xOffset;

        // 当ToolTip超出屏幕底部时，上移与target底部对齐
        if (targetPos.y - (toolTipHeight + targetHeight) / 2f < 0)
            targetPos.y = targetPos.y + (toolTipHeight - targetHeight) / 2f + yOffset;
        else // 默认顶部对齐
            targetPos.y = targetPos.y - (toolTipHeight - targetHeight) / 2f - yOffset;

        rect.position = targetPos;
    }

    // 带有颜色的字体
    public string GetColorText(string colorHex, string text)
    {
        return $"<color={colorHex}>{text}</color>";
    }
}
