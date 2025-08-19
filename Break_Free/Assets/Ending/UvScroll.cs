using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UvScroll : MonoBehaviour
{
    [Tooltip("每秒UV偏移量（负数向左，正数向右）")]
    public float speed = -0.2f;

    [Tooltip("横向可见的平铺次数（1=正好一张贴图宽度）")]
    public float tileX = 1f;

    private RawImage raw;
    private Rect uv;

    void Awake()
    {
        raw = GetComponent<RawImage>();
        uv = raw.uvRect;

        // 控制可见范围内的平铺密度：tileX 越大，单位宽度内重复越多
        uv.width = Mathf.Max(0.0001f, tileX);
        uv.height = 1f;
        raw.uvRect = uv;
    }

    void Update()
    {
        uv = raw.uvRect;
        uv.x = Mathf.Repeat(uv.x + speed * Time.deltaTime, 1f);
        raw.uvRect = uv;
    }
}
