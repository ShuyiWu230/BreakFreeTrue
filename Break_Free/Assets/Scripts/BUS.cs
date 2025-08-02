using UnityEngine;

public class BUS : MonoBehaviour
{
    public float amplitude = 0.05f;   // �������ȣ����� 0.01 ~ 0.05��
    public float frequency = 1f;      // ����Ƶ�ʣ����� 0.3 ~ 1.5��

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
