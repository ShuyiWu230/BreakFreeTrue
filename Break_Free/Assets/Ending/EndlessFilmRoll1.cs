using UnityEngine;
using UnityEngine.UI;

public class EndlessFilmRoll1 : MonoBehaviour
{
    [Header("滚动速度（像素/秒）")]
    public float scrollSpeed = 100f;

    [Header("向左滚动？（否则向右）")]
    public bool moveLeft = true;

    private RectTransform rectTransform;   // 自身Rect
    private RectTransform parentRect;      // 父级（通常是Canvas或容器）
    private Vector2 startPosition;         // 初始锚点位置
    private float horizontalBound;         // 屏幕/容器的水平边界（宽度的一半概念）

    void Start()
    {
        // 缓存组件
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found on the GameObject.");
            enabled = false;
            return;
        }

        // 这里获取父RectTransform（注意：需要父级是RectTransform）
        parentRect = transform.parent as RectTransform;
        if (parentRect == null)
        {
            Debug.LogError("Parent is not a RectTransform.");
            enabled = false;
            return;
        }

        // 记录初始位置
        startPosition = rectTransform.anchoredPosition;

        // 水平边界：用父容器的宽度。你的原脚本是用高度做垂直边界，这里换成宽度即可
        horizontalBound = parentRect.rect.width;
    }

    void Update()
    {
        // 计算方向：向左为负，向右为正
        float dir = moveLeft ? -1f : 1f;

        // 沿X轴移动
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x += dir * scrollSpeed * Time.deltaTime;
        rectTransform.anchoredPosition = pos;

        // 超出右侧 -> 从左边进；超出左侧 -> 从右边进
        if (rectTransform.anchoredPosition.x > horizontalBound)
        {
            rectTransform.anchoredPosition = new Vector2(-horizontalBound, startPosition.y);
        }
        else if (rectTransform.anchoredPosition.x < -horizontalBound)
        {
            rectTransform.anchoredPosition = new Vector2(horizontalBound, startPosition.y);
        }
    }
}
