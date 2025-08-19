using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UvScroll : MonoBehaviour
{
    [Tooltip("ÿ��UVƫ���������������������ң�")]
    public float speed = -0.2f;

    [Tooltip("����ɼ���ƽ�̴�����1=����һ����ͼ��ȣ�")]
    public float tileX = 1f;

    private RawImage raw;
    private Rect uv;

    void Awake()
    {
        raw = GetComponent<RawImage>();
        uv = raw.uvRect;

        // ���ƿɼ���Χ�ڵ�ƽ���ܶȣ�tileX Խ�󣬵�λ������ظ�Խ��
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
