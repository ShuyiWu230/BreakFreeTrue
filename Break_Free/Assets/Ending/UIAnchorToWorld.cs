using UnityEngine;
using UnityEngine.UI;

public class UIAnchorToWorld : MonoBehaviour
{
    public Transform worldTarget;     // �ؿ��յ�����
    public RectTransform uiImage;     // ��ȪUI��RectTransform
    public Camera mainCam;

    void LateUpdate()
    {
        if (!worldTarget || !uiImage || !mainCam) return;
        Vector3 screen = mainCam.WorldToScreenPoint(worldTarget.position);
        uiImage.position = screen;   // ��UI������������Ļλ��
    }
}
