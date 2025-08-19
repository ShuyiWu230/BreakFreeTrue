using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HourglassController : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool IsCanControl;
    public float moveDistance = 2f;   // 每次移动距离
    public float flipDuration = 0.5f; // 翻转动画时间

    [Header("Sand Settings")]
    public float sandDrainSpeed = 0.3f; // 沙子正常流失速度
    public float leakSpeed = 0.15f;     // 沙子泄漏速度

    [Header("Fish Settings")]
    public Transform fish;             // 小鱼对象
    public Color normalColor = Color.white; // 正常颜色
    public Color warningColor = Color.red;  // 警告颜色

    [Header("Water Drop Settings")]
    public GameObject waterDropPrefab; // 水滴预制体
    public Transform leakPoint;        // 漏水点（水滴生成位置）
    public float dropSpawnRate = 1f;   // 每秒生成水滴数量（1=1个/秒，2=2个/秒）
    public float dropLifetime = 2f;    // 水滴存在时间（秒）
    public Vector2 dropDirection = new Vector2(0, -1); // 水滴初始方向（向下）
    public float dropSpeed = 1f;       // 水滴初始速度

    [Header("Recovery Settings")]
    public string recoveryItemTag = "RecoveryItem"; // 恢复物品标签
    public GameObject recoveryEffect; // 恢复特效
    [Header("Key Settings")]
    public bool HasKey;
    [Header("Slope Settings")]
    public float slopeRaycastDistance = 1f; // 斜坡检测射线距离
    public float slopeRaycastOffset = 0.5f; // 斜坡检测水平偏移
    public LayerMask slopeLayer; // 斜坡图层
    public float slopeRotationSpeed = 10f; // 斜坡旋转速度
    public float maxSlopeAngle = 45f; // 最大斜坡角度

    // 内部变量
    private Transform sandTop;
    private Transform sandBottom;
    private bool isFlipping = false;
    private bool isUpright = true;
    private float sandInTop = 1f;
    private float sandInBottom = 0f;
    private Renderer fishRenderer;
    private bool isLeaking = false;
    private float dropSpawnTimer = 0f; // 水滴生成计时器
    private FlipSoundPlayer flipSoundPlayer;
    // 斜坡旋转相关
    private bool isOnSlope = false;
    private float targetRotation = 0f;
    private float originalRotation = 0f;
    private float currentVelocity;
    private float smoothTime = 0.1f;



    [Header("Fish energyBar Settings")]
    //小鱼精力条
    public float energyBarLongth;//长度=100
    float energyCurrentLongth;//目前的长度
    bool inWater;//小鱼状态
    float HPcurrent = 100;
    float maxHP = 100;
    //public TextMesh HP,Energy;
    public Text EnergyText, WaterText, HPText;


    //新增功能：碰到tag为handle的碰撞体就变成该碰撞体的子物体
    //血量为零时出现失败界面：
    public GameObject panel;

    void Start()
    {
        IsCanControl = true;
        // 获取沙子对象
        Transform sandContainer = transform.Find("SandContainer");
        sandTop = sandContainer.Find("SandTop");
        sandBottom = sandContainer.Find("SandBottom");

        // 获取小鱼渲染器
        fishRenderer = fish.GetComponent<Renderer>();

        // 初始状态
        UpdateSandVisual();
        UpdateFishColor();
        UpdateLeakState();

        //重置精力条
        energyCurrentLongth = energyBarLongth;
        flipSoundPlayer = GetComponent<FlipSoundPlayer>();

        this.tag = "Player";
        Time.timeScale = 1;
        // 斜坡旋转初始化
        originalRotation = transform.eulerAngles.z;
        targetRotation = originalRotation;
    }

    void Update()
    {

        //Hp为零判负：
        if (HPcurrent <= 0)
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
        }

        //水量检测ui
        WaterText.text = ((sandInTop + sandInBottom) * 100).ToString("F0") + "%";

        if (IsCanControl)
        {
            // 沙子流失逻辑
            if (!isFlipping)
            {
                // 沙子从顶部流到底部
                if (sandInTop > 0)
                {
                    float drainAmount = sandDrainSpeed * Time.deltaTime;
                    sandInTop = Mathf.Clamp01(sandInTop - drainAmount);
                    sandInBottom = Mathf.Clamp01(sandInBottom + drainAmount);
                }

                // 处理泄漏逻辑（沙子减少）
                if (isLeaking && GetCurrentFishSectionSand() > 0)
                {
                    float leakAmount = leakSpeed * Time.deltaTime;
                    DecreaseCurrentFishSectionSand(leakAmount);
                }

                // 水滴生成逻辑（仅在漏水时）
                if (isLeaking && leakPoint != null && waterDropPrefab != null)
                {
                    dropSpawnTimer += Time.deltaTime;
                    float spawnInterval = 1f / dropSpawnRate; // 生成间隔（秒）

                    if (dropSpawnTimer >= spawnInterval)
                    {
                        SpawnWaterDrop();
                        dropSpawnTimer = 0f; // 重置计时器
                    }
                }

                UpdateSandVisual();
                UpdateFishColor();
            }

            // 键盘控制翻转
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                StartFlip(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                StartFlip(1);
            }

            //检测鱼状态控制精力条和血量
            FishInWaterTrack();
        }

    }

    void FixedUpdate()
    {
        // 斜坡检测和旋转应用
        DetectSlope();
        ApplySlopeRotation();
    }
    // 斜坡检测
    void DetectSlope()
    {
        // 如果正在翻转，不进行斜坡检测
        if (isFlipping) return;

        Vector2 rayOrigin = transform.position;
        bool leftHit = Physics2D.Raycast(rayOrigin + Vector2.left * slopeRaycastOffset, Vector2.down,
                                        slopeRaycastDistance, slopeLayer);
        bool rightHit = Physics2D.Raycast(rayOrigin + Vector2.right * slopeRaycastOffset, Vector2.down,
                                        slopeRaycastDistance, slopeLayer);

        // 优先使用右侧射线结果
        RaycastHit2D hit = rightHit ?
            Physics2D.Raycast(rayOrigin + Vector2.right * slopeRaycastOffset, Vector2.down,
                              slopeRaycastDistance, slopeLayer) :
            Physics2D.Raycast(rayOrigin + Vector2.left * slopeRaycastOffset, Vector2.down,
                              slopeRaycastDistance, slopeLayer);

        // 如果检测到斜坡
        if (hit.collider != null)
        {
            float slopeAngle = CalculateSlopeAngle(hit.normal);

            // 如果斜坡角度在可攀爬范围内
            if (Mathf.Abs(slopeAngle) <= maxSlopeAngle)
            {
                isOnSlope = true;

                // 根据沙漏状态调整目标角度
                if (isUpright)
                {
                    targetRotation = slopeAngle;
                }
                else
                {
                    // 倒立状态需要额外旋转180度
                    targetRotation = slopeAngle + 180f;
                }
                return;
            }
        }

        // 不在斜坡上或斜坡角度过大
        isOnSlope = false;

        // 根据沙漏状态设置目标角度
        targetRotation = isUpright ? 0f : 180f;
    }

    // 计算斜坡角度
    float CalculateSlopeAngle(Vector2 normal)
    {
        // 计算法线与垂直方向之间的角度
        float angle = Vector2.Angle(normal, Vector2.up);

        // 根据法线方向确定正负角度
        if (normal.x > 0)
            angle = -angle;

        return angle;
    }

    // 应用斜坡旋转
    void ApplySlopeRotation()
    {
        // 如果正在翻转，不应用斜坡旋转
        if (isFlipping) return;

        float currentZRotation = transform.eulerAngles.z;

        // 将角度转换为-180到180的范围
        if (currentZRotation > 180)
            currentZRotation -= 360;

        // 使用SmoothDampAngle平滑旋转
        float newZRotation = Mathf.SmoothDampAngle(
            currentZRotation,
            targetRotation,
            ref currentVelocity,
            smoothTime
        );

        // 应用旋转
        transform.rotation = Quaternion.Euler(0, 0, newZRotation);
    }


    // 生成水滴
    void SpawnWaterDrop()
    {
        // 实例化水滴
        GameObject drop = Instantiate(waterDropPrefab, leakPoint.position, Quaternion.identity);

        // 给水滴添加初速度
        Rigidbody2D rb = drop.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = dropDirection * dropSpeed;
        }

        // 设置水滴自动销毁
        Destroy(drop, dropLifetime);
    }

    // 更新沙子视觉效果
    void UpdateSandVisual()
    {
        if (isUpright)
        {
            sandTop.localScale = new Vector3(1, sandInTop, 1);
            sandBottom.localScale = new Vector3(1, sandInBottom, 1);
        }
        else
        {
            sandTop.localScale = new Vector3(1, sandInBottom, 1);
            sandBottom.localScale = new Vector3(1, sandInTop, 1);
        }
    }

    // 更新小鱼颜色
    void UpdateFishColor()
    {
        bool isInTopSection = (isUpright && fish.position.y > transform.position.y) ||
                             (!isUpright && fish.position.y < transform.position.y);

        if (isInTopSection)
        {
            float currentTopSandPercentage = isUpright ? sandInTop : sandInBottom;
            fishRenderer.material.color = currentTopSandPercentage < 0.7f ? warningColor : normalColor;

            //新增bool iswater检测小鱼状态
            inWater = currentTopSandPercentage < 0.7f ? false : true;
        }
        else
        {
            fishRenderer.material.color = normalColor;
        }
    }

    // 更新泄漏状态
    void UpdateLeakState()
    {
        bool isInTopSection = (isUpright && fish.position.y > transform.position.y) ||
                             (!isUpright && fish.position.y < transform.position.y);

        isLeaking = (isInTopSection && !isUpright) || (!isInTopSection && isUpright);
    }

    // 获取小鱼所在部分的沙子量
    float GetCurrentFishSectionSand()
    {
        bool isInTopSection = (isUpright && fish.position.y > transform.position.y) ||
                             (!isUpright && fish.position.y < transform.position.y);

        return isInTopSection ? (isUpright ? sandInTop : sandInBottom) : (isUpright ? sandInBottom : sandInTop);
    }

    // 减少小鱼所在部分的沙子量
    void DecreaseCurrentFishSectionSand(float amount)
    {
        bool isInTopSection = (isUpright && fish.position.y > transform.position.y) ||
                             (!isUpright && fish.position.y < transform.position.y);

        if (isInTopSection)
        {
            if (isUpright) sandInTop = Mathf.Clamp01(sandInTop - amount);
            else sandInBottom = Mathf.Clamp01(sandInBottom - amount);
        }
        else
        {
            if (isUpright) sandInBottom = Mathf.Clamp01(sandInBottom - amount);
            else sandInTop = Mathf.Clamp01(sandInTop - amount);
        }
    }

    // 恢复顶部沙子
    public void RestoreTopSand()
    {
        if (isUpright)
        {
            sandInTop = 1f;
            sandInBottom = 0f;
        }
        else
        {
            sandInTop = 0f;
            sandInBottom = 1f;
        }

        UpdateSandVisual();
        UpdateFishColor();
        UpdateLeakState();

        if (recoveryEffect != null)
        {
            recoveryEffect.SetActive(true);
            Invoke("DisableRecoveryEffect", 0.5f);
        }
    }

    void DisableRecoveryEffect()
    {
        recoveryEffect.SetActive(false);
    }

    // 开始翻转
    void StartFlip(int direction)
    {
        if (!isFlipping)
        {
            if (flipSoundPlayer != null)
                flipSoundPlayer.PlayFlipSound();  // 💡 在翻转前播放音效！

            StartCoroutine(FlipCoroutine(direction));
        }
    }


    // 翻转动画协程
    System.Collections.IEnumerator FlipCoroutine(int direction)
    {
        isFlipping = true;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 targetPos = startPos + new Vector3(direction * moveDistance, 0, 0);
        Quaternion targetRot = startRot * Quaternion.Euler(0, 0, 180 * direction);

        float elapsed = 0f;
        while (elapsed < flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / flipDuration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
        isUpright = !isUpright;
        (sandInTop, sandInBottom) = (sandInBottom, sandInTop);
        UpdateSandVisual();
        UpdateFishColor();
        UpdateLeakState();
        isFlipping = false;
    }

    // 碰撞检测
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(recoveryItemTag))
        {
            RestoreTopSand();
            Destroy(other.gameObject);
        }
    }




    void FishInWaterTrack()
    {
        //如果小鱼不在水里，减能量条
        if (inWater == false && energyCurrentLongth >= 0)
        {
            energyCurrentLongth -= 20 * Time.deltaTime;
        }

        if (energyCurrentLongth > 100) energyCurrentLongth = 100;
        if (energyCurrentLongth < 0) energyCurrentLongth = 0;


        //如果小鱼在水里，恢复能量条
        if (inWater == true && energyCurrentLongth <= 100)
        {
            energyCurrentLongth += 45 * Time.deltaTime;
        }

        //如果精力条为0，则减血量：
        if (energyCurrentLongth <= 0)
        {
            HPcurrent -= 20 * Time.deltaTime;
        }

        HPText.text = "HP: " + HPcurrent.ToString("F0") + "%";

        EnergyText.text = energyCurrentLongth.ToString("F0") + "%";

        if (HPcurrent < 0) HPcurrent = 0;
    }


    //当碰到把手时：
    private void OnCollisionEnter2D(Collision2D collision)
    {
        tag = collision.collider.tag;
        if (tag == "handle")
        {


            transform.SetParent(collision.transform.parent);
            //transform.eulerAngles += new Vector3(0, 0, collision.transform.parent.GetComponent<SceneRotationController>().targetRotation);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        tag = collision.collider.tag;
        if (tag == "handle")
        {

            transform.SetParent(null);

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = isOnSlope ? Color.green : Color.red;

        Vector2 origin = transform.position;
        Gizmos.DrawLine(origin + Vector2.left * slopeRaycastOffset,
                       origin + Vector2.left * slopeRaycastOffset + Vector2.down * slopeRaycastDistance);
        Gizmos.DrawLine(origin + Vector2.right * slopeRaycastOffset,
                       origin + Vector2.right * slopeRaycastOffset + Vector2.down * slopeRaycastDistance);
    }

}