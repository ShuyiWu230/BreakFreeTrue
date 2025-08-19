using UnityEngine;
using UnityEngine.UI;

public class UIAnchorToWorld : MonoBehaviour
{
    public Transform worldTarget;     // 关卡终点物体
    public RectTransform uiImage;     // 喷泉UI的RectTransform
    public Camera mainCam;

    void LateUpdate()
    {
        if (!worldTarget || !uiImage || !mainCam) return;
        Vector3 screen = mainCam.WorldToScreenPoint(worldTarget.position);
        uiImage.position = screen;   // 让UI跟随世界点的屏幕位置
    }
}
