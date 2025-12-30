using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour, IDragHandler
{
    protected RectTransform rect; // 当前UI的坐标

    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 中心锚点位置加上鼠标移动的值
        rect.anchoredPosition += eventData.delta;
    }
}
