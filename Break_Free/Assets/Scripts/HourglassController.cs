using UnityEngine;

public class HourglassController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2f;   // 每次移动距离
    public float flipDuration = 0.5f; // 翻转动画时间

    [Header("Sand Settings")]
    public float sandDrainSpeed = 0.3f; // 沙子流失速度

    [Header("Fish Settings")]
    public Transform fish;             // 小鱼对象
    public Color normalColor = Color.white; // 正常颜色
    public Color warningColor = Color.red;  // 警告颜色

    // 子对象引用
    private Transform sandTop;
    private Transform sandBottom;
    private bool isFlipping = false;
    private bool isUpright = true;   // 是否正立状态
    private float sandInTop = 1f;     // 顶部沙子量(0-1)
    private float sandInBottom = 0f;  // 底部沙子量(0-1)
    private Renderer fishRenderer;

    void Start()
    {
        // 获取沙子对象
        Transform sandContainer = transform.Find("SandContainer");
        sandTop = sandContainer.Find("SandTop");
        sandBottom = sandContainer.Find("SandBottom");
        
        // 获取小鱼渲染器
        fishRenderer = fish.GetComponent<Renderer>();
        
        // 初始状态
        UpdateSandVisual();
        UpdateFishColor();
    }

    void Update()
    {
        // 沙子流失逻辑
        if (!isFlipping)
        {
            // 沙子始终从顶部流到底部
            if (sandInTop > 0)
            {
                float drainAmount = sandDrainSpeed * Time.deltaTime;
                sandInTop = Mathf.Clamp01(sandInTop - drainAmount);
                sandInBottom = Mathf.Clamp01(sandInBottom + drainAmount);
                UpdateSandVisual();
                UpdateFishColor();
            }
        }

        // 键盘控制
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            StartFlip(-1); // 向左翻转
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            StartFlip(1); // 向右翻转
        }
    }

    void UpdateSandVisual()
    {
        // 根据状态显示沙子
        if (isUpright)
        {
            // 正立状态：顶部沙子显示为SandTop，底部沙子显示为SandBottom
            sandTop.localScale = new Vector3(1, sandInTop, 1);
            sandBottom.localScale = new Vector3(1, sandInBottom, 1);
        }
        else
        {
            // 倒立状态：顶部沙子显示为SandBottom，底部沙子显示为SandTop
            sandTop.localScale = new Vector3(1, sandInBottom, 1);
            sandBottom.localScale = new Vector3(1, sandInTop, 1);
        }
    }

    void UpdateFishColor()
    {
        // 判断小鱼当前所在位置
        bool isInTopSection = (isUpright && fish.position.y > transform.position.y) ||
                             (!isUpright && fish.position.y < transform.position.y);
        
        // 如果小鱼在上半部分，检查沙子量
        if (isInTopSection)
        {
            // 获取当前顶部沙子量的百分比
            float currentTopSandPercentage = isUpright ? sandInTop : sandInBottom;
            
            // 如果沙子量小于70%，小鱼变红
            fishRenderer.material.color = currentTopSandPercentage < 0.7f ? warningColor : normalColor;
        }
        else
        {
            // 小鱼在下半部分，恢复正常颜色
            fishRenderer.material.color = normalColor;
        }
    }

    void StartFlip(int direction)
    {
        if (!isFlipping)
        {
            StartCoroutine(FlipCoroutine(direction));
        }
    }

    System.Collections.IEnumerator FlipCoroutine(int direction)
    {
        isFlipping = true;
        
        // 保存初始状态
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        
        // 计算目标位置和旋转
        Vector3 targetPos = startPos + new Vector3(direction * moveDistance, 0, 0);
        Quaternion targetRot = startRot * Quaternion.Euler(0, 0, 180 * direction);
        
        // 动画进度
        float elapsed = 0f;
        
        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / flipDuration);
            
            // 移动和旋转插值
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            
            yield return null;
        }
        
        // 完成翻转
        transform.rotation = targetRot;
        
        // 切换状态并交换沙子量
        isUpright = !isUpright;
        float temp = sandInTop;
        sandInTop = sandInBottom;
        sandInBottom = temp;
        UpdateSandVisual();
        UpdateFishColor();
        
        isFlipping = false;
    }
}