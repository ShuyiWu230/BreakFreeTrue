using UnityEngine;
using UnityEngine.UI;

public class EndlessFilmRoll1 : MonoBehaviour
{
    [Header("�����ٶȣ�����/�룩")]
    public float scrollSpeed = 100f;

    [Header("������������������ң�")]
    public bool moveLeft = true;

    private RectTransform rectTransform;   // ����Rect
    private RectTransform parentRect;      // ������ͨ����Canvas��������
    private Vector2 startPosition;         // ��ʼê��λ��
    private float horizontalBound;         // ��Ļ/������ˮƽ�߽磨��ȵ�һ����

    void Start()
    {
        // �������
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found on the GameObject.");
            enabled = false;
            return;
        }

        // �����ȡ��RectTransform��ע�⣺��Ҫ������RectTransform��
        parentRect = transform.parent as RectTransform;
        if (parentRect == null)
        {
            Debug.LogError("Parent is not a RectTransform.");
            enabled = false;
            return;
        }

        // ��¼��ʼλ��
        startPosition = rectTransform.anchoredPosition;

        // ˮƽ�߽磺�ø������Ŀ�ȡ����ԭ�ű����ø߶�����ֱ�߽磬���ﻻ�ɿ�ȼ���
        horizontalBound = parentRect.rect.width;
    }

    void Update()
    {
        // ���㷽������Ϊ��������Ϊ��
        float dir = moveLeft ? -1f : 1f;

        // ��X���ƶ�
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x += dir * scrollSpeed * Time.deltaTime;
        rectTransform.anchoredPosition = pos;

        // �����Ҳ� -> ����߽���������� -> ���ұ߽�
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
