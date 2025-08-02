using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // ÿ���ٶȲ�ͬ
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        // ��ȡ����
        mat = GetComponent<Renderer>().material;
        offset = mat.mainTextureOffset;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
