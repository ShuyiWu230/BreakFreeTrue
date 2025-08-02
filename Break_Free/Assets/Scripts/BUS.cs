using UnityEngine;

public class BUS : MonoBehaviour
{
    public float amplitude = 0.05f;   // 浮动幅度（建议 0.01 ~ 0.05）
    public float frequency = 1f;      // 浮动频率（建议 0.3 ~ 1.5）

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = initialPosition + new Vector3(0f, offsetY, 0f);
    }
}
