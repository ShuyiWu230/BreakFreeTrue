using UnityEngine;

public class FadeOutItem : MonoBehaviour
{
    [Header("消失设置")]
    public float fadeDuration = 2f;
    public float startDelay = 1f; // 出现后多久开始消失
    public int itemIndex = 1; // 物品序号（由管理器设置）
    public bool autoStart = true; // 是否自动开始消失

    private SpriteRenderer spriteRenderer;
    private ItemSequenceManager manager;
    private bool isFading = false;

    // 公开属性用于检查状态
    public bool IsFading => isFading;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<ItemSequenceManager>();

        // 如果设置为自动开始，则延迟后开始淡出
        if (autoStart)
        {
            Invoke("StartFade", startDelay);
        }
    }

    // 公开方法用于外部启动淡出
    public void StartFade()
    {
        if (!isFading)
        {
            Invoke("BeginFadeProcess", startDelay);
        }
    }

    private void BeginFadeProcess()
    {
        isFading = true;
    }

    void Update()
    {
        if (!isFading) return;

        // 淡出逻辑
        Color color = spriteRenderer.color;
        color.a -= Time.deltaTime / fadeDuration;
        spriteRenderer.color = color;

        // 完全消失后处理
        if (color.a <= 0)
        {
            isFading = false;

            // 通知管理器
            if (manager != null)
            {
                manager.OnItemFaded(itemIndex);
            }

            // 禁用当前物体
            gameObject.SetActive(false);
        }
    }
}