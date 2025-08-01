using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("缩放设置")]
    public float zoomSpeed = 5f;       // 缩放速度
    public float minZoom = 2f;         // 最小缩放值
    public float maxZoom = 10f;        // 最大缩放值

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
            mainCamera = Camera.main; // 获取主摄像机
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // 计算新缩放值（滚轮向上缩小size=放大视野）
            float newSize = mainCamera.orthographicSize - scroll * zoomSpeed;

            // 限制缩放范围
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            // 应用缩放
            mainCamera.orthographicSize = newSize;
        }
    }
}