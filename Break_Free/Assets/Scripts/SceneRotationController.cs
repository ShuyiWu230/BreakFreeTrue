using UnityEngine;

public class SceneRotationController : MonoBehaviour
{
    [Header("旋转设置")]
    [SerializeField] public float targetRotation = 10f; // 目标旋转角度
    [SerializeField] private float rotationDuration = 1f; // 旋转持续时间（秒）

    [Header("状态控制")]
    [Tooltip("设置为true开始旋转到目标角度，false回到0度")]
    public bool shouldRotate = false;

    private float startRotation; // 初始旋转角度
    private float currentRotationAngle; // 当前旋转角度
    private float rotationTimer; // 旋转计时器
    private bool isRotating; // 是否正在旋转

    GameManager refToManager;
    void Start()
    {
        //挂manager
        refToManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 初始化旋转状态
        startRotation = transform.eulerAngles.z;
        currentRotationAngle = startRotation;
        rotationTimer = 0f;
        isRotating = false;
    }

    void Update()
    {
        //是否需要旋转由Manager统一控制
        shouldRotate = refToManager.shouldRotate;

        // 检测状态变化
        if ((shouldRotate && currentRotationAngle != targetRotation) ||
            (!shouldRotate && currentRotationAngle != startRotation))
        {
            isRotating = true;
            rotationTimer += Time.deltaTime;

            // 计算插值比例 (0-1)
            float t = Mathf.Clamp01(rotationTimer / rotationDuration);

            // 应用平滑插值
            if (shouldRotate)
            {
                currentRotationAngle = Mathf.Lerp(startRotation, targetRotation, SmoothStep(t));
            }
            else
            {
                currentRotationAngle = Mathf.Lerp(targetRotation, startRotation, SmoothStep(t));
            }

            // 应用旋转
            transform.rotation = Quaternion.Euler(0, 0, currentRotationAngle);
        }
        else
        {
            // 重置计时器
            rotationTimer = 0f;
            isRotating = false;
        }
    }

    // 自定义缓动函数（平滑过渡）
    private float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }


}